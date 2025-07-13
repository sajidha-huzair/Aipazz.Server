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
            // ───── 1. load current invoice ─────
            var invoice = await _repo.GetByIdAsync(cmd.Invoice.Id, cmd.Invoice.UserId)
                         ?? throw new Exception("Invoice not found");

            // ───── 2. map editable fields from UI ─────
            // Match the fields visible in your screenshot
            invoice.IssueDate = cmd.Invoice.IssueDate;
            invoice.DueDate = cmd.Invoice.DueDate;
            invoice.FooterNotes = MergeFooter(
    cmd.Invoice.FooterNotes,                       // the discount note typed in UI
    $"Please make all amounts payable to: Law Office of {_user.FullName}"  // default
);
            // "Discount note" / footer
            invoice.PaymentProfileNotes = cmd.Invoice.PaymentProfileNotes;   // "Invoice note"
            invoice.Status = cmd.Invoice.Status;                // if editable
            invoice.Currency = cmd.Invoice.Currency;              // if you added this column
            invoice.Subject = cmd.Invoice.Subject;               // optional
            invoice.DiscountValue = cmd.Invoice.DiscountValue;         // numeric
            invoice.DiscountType = cmd.Invoice.DiscountType;          // "%" or "₹"

            invoice.UpdatedBy = cmd.Invoice.UpdatedBy;
            invoice.UpdatedAt = DateTime.UtcNow;

            // If UI allows adding/removing entries, sync the EntryIds list
            invoice.EntryIds = cmd.Invoice.EntryIds;

            // ───── 3. reload linked entries ─────
            var timeEntries = await _timeRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, invoice.UserId);
            var expenseEntries =
                await _expenseRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, invoice.UserId);

            // ───── 4. recalc totals & discount ─────
            var rawTotal = timeEntries.Sum(t => t.Amount) + expenseEntries.Sum(e => e.Amount);

            var discount = invoice.DiscountType == "%"
                ? rawTotal * (invoice.DiscountValue / 100m)
                : invoice.DiscountValue;

            invoice.TotalAmount = rawTotal - discount;

            // ───── 5. delete old PDF & regenerate ─────
            if (!string.IsNullOrWhiteSpace(invoice.PdfFileUrl))
                await _blob.DeletePdfAsync(invoice.PdfFileUrl);   // ignore result

            var pdfBytes = await _pdf.GeneratePdfAsync(invoice, timeEntries, expenseEntries);

            invoice.PdfFileUrl =
                await _blob.SavePdfAsync(invoice.UserId, invoice.id, pdfBytes); // overwrite

            // ───── 6. persist ─────
            await _repo.UpdateAsync(invoice);

            return Unit.Value;
        }

        private static string MergeFooter(string discountNote, string defaultFooter)
        {
            if (string.IsNullOrWhiteSpace(discountNote))
                return defaultFooter;                       // nothing typed -> keep original

            discountNote = discountNote.Trim();

            // add a period if user did not supply one
            if (!discountNote.EndsWith("."))
                discountNote += ".";

            // final footer:  "All discount has been deducted.\nPlease make all amounts payable …"
            return $"{discountNote}\n{defaultFooter}";
        }
    }
}
