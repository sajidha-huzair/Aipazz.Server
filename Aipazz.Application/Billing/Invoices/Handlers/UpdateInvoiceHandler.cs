using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Common.Aipazz.Application.Common;
using Aipazz.Domian.Billing;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
    {
        private readonly IInvoiceRepository _repo;
        private readonly IInvoicePdfGenerator _pdf;
        private readonly IInvoiceBlobService _blob;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;
        private readonly IUserContext _user;

        public UpdateInvoiceHandler(
            IInvoiceRepository repo,
            IInvoicePdfGenerator pdf,
            IInvoiceBlobService blob,
            ITimeEntryRepository timeRepo,
            IExpenseEntryRepository expenseRepo,
             IUserContext userContext)
        {
            _repo = repo;
            _pdf = pdf;
            _blob = blob;
            _timeRepo = timeRepo;
            _expenseRepo = expenseRepo;
            _user = userContext;
        }

        public async Task<Unit> Handle(UpdateInvoiceCommand cmd, CancellationToken ct)
        {
            Console.WriteLine("=== UPDATE INVOICE HANDLER START ===");
            Console.WriteLine($"Invoice ID: {cmd.Invoice.Id}");
            Console.WriteLine($"User ID: {cmd.Invoice.UserId}");
            Console.WriteLine($"Entry IDs received: [{string.Join(", ", cmd.Invoice.EntryIds)}]");

            // ───── 1. load current invoice ─────
            var invoice = await _repo.GetByIdAsync(cmd.Invoice.Id, cmd.Invoice.UserId)
                         ?? throw new Exception("Invoice not found");

            Console.WriteLine($"Current invoice total: {invoice.TotalAmount}");
            Console.WriteLine($"Current invoice entry IDs: [{string.Join(", ", invoice.EntryIds)}]");

            // ───── 2. map editable fields from UI ─────
            invoice.IssueDate = cmd.Invoice.IssueDate;
            invoice.DueDate = cmd.Invoice.DueDate;
            invoice.FooterNotes = MergeFooter(cmd.Invoice.FooterNotes, $"Please make all amounts payable to: Law Office of {_user.FullName}");
            invoice.PaymentProfileNotes = cmd.Invoice.PaymentProfileNotes;
            invoice.Status = cmd.Invoice.Status;
            invoice.Currency = cmd.Invoice.Currency;
            invoice.Subject = cmd.Invoice.Subject;
            invoice.DiscountValue = cmd.Invoice.DiscountValue;
            invoice.DiscountType = cmd.Invoice.DiscountType;
            invoice.UpdatedBy = cmd.Invoice.UpdatedBy;
            invoice.UpdatedAt = DateTime.UtcNow;

            // Update EntryIds
            invoice.EntryIds = cmd.Invoice.EntryIds;
            Console.WriteLine($"Updated EntryIds in invoice: [{string.Join(", ", invoice.EntryIds)}]");

            // ───── 3. Use GetAllEntriesByIdsAsync directly (SIMPLIFIED APPROACH) ─────
            Console.WriteLine("=== FETCHING ENTRIES BY IDS ===");

            var timeEntries = await _timeRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, invoice.UserId);
            var expenseEntries = await _expenseRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, invoice.UserId);

            Console.WriteLine($"Time entries retrieved: {timeEntries.Count()}");
            foreach (var te in timeEntries)
            {
                Console.WriteLine($"  ✓ Time Entry: {te.id} - Amount: {te.Amount}");
            }

            Console.WriteLine($"Expense entries retrieved: {expenseEntries.Count()}");
            foreach (var ee in expenseEntries)
            {
                Console.WriteLine($"  ✓ Expense Entry: {ee.id} - Amount: {ee.Amount}");
            }

            // Check if any entry IDs are missing
            var foundTimeIds = timeEntries.Select(t => t.id).ToList();
            var foundExpenseIds = expenseEntries.Select(e => e.id).ToList();
            var allFoundIds = foundTimeIds.Concat(foundExpenseIds).ToList();

            var missingIds = invoice.EntryIds.Except(allFoundIds).ToList();
            if (missingIds.Any())
            {
                Console.WriteLine($"WARNING: Missing entry IDs: [{string.Join(", ", missingIds)}]");

                // Log each missing ID for debugging
                foreach (var missingId in missingIds)
                {
                    Console.WriteLine($"✗ Missing Entry ID: {missingId}");
                }
            }

            // ───── 4. recalc totals & discount WITH DETAILED LOGGING ─────
            var timeTotal = timeEntries.Sum(t => t.Amount);
            var expenseTotal = expenseEntries.Sum(e => e.Amount);
            var rawTotal = timeTotal + expenseTotal;

            Console.WriteLine($"Time entries total: {timeTotal}");
            Console.WriteLine($"Expense entries total: {expenseTotal}");
            Console.WriteLine($"Raw total: {rawTotal}");

            var discount = invoice.DiscountType == "%"
                ? rawTotal * (invoice.DiscountValue / 100m)
                : invoice.DiscountValue;

            Console.WriteLine($"Discount value: {invoice.DiscountValue} {invoice.DiscountType}");
            Console.WriteLine($"Calculated discount: {discount}");

            invoice.TotalAmount = rawTotal - discount;
            Console.WriteLine($"Final total amount: {invoice.TotalAmount}");

            // ───── 5. delete old PDF & regenerate ─────
            if (!string.IsNullOrWhiteSpace(invoice.PdfFileUrl))
                await _blob.DeletePdfAsync(invoice.PdfFileUrl);

            var pdfBytes = await _pdf.GeneratePdfAsync(invoice, timeEntries, expenseEntries);
            invoice.PdfFileUrl = await _blob.SavePdfAsync(invoice.UserId, invoice.id, pdfBytes);

            // ───── 6. persist ─────
            await _repo.UpdateAsync(invoice);

            Console.WriteLine($"Invoice updated successfully with total: {invoice.TotalAmount}");
            Console.WriteLine("=== UPDATE INVOICE HANDLER END ===");

            return Unit.Value;
        }

        private static string MergeFooter(string discountNote, string defaultFooter)
        {
            if (string.IsNullOrWhiteSpace(discountNote))
                return defaultFooter;

            discountNote = discountNote.Trim();

            // Check if the discountNote already contains the default footer to avoid duplication
            if (discountNote.Contains("Please make all amounts payable to:"))
            {
                return discountNote; // Return as-is if it already contains the default footer
            }

            // add a period if user did not supply one
            if (!discountNote.EndsWith("."))
                discountNote += ".";

            // final footer:  "All discount has been deducted.\nPlease make all amounts payable …"
            return $"{discountNote}\n{defaultFooter}";
        }
    }
}