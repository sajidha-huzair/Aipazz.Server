using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.client.Interfaces;
using Aipazz.Application.Common;
using Aipazz.Application.Common.Aipazz.Application.Common;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Billing;
using MediatR;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class GenerateInvoiceFromSelectionHandler
        : IRequestHandler<GenerateInvoiceFromSelectionCommand, string>
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMatterRepository _matterRepo;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IInvoicePdfGenerator _pdfGenerator;   // ← NEW
        private readonly IInvoiceBlobService _invoiceBlob;
        private readonly IUserContext _user;

        public GenerateInvoiceFromSelectionHandler(
            IClientRepository clientRepo,
            IMatterRepository matterRepo,
            ITimeEntryRepository timeRepo,
            IExpenseEntryRepository expenseRepo,
            IInvoiceRepository invoiceRepo,
            IUserContext userContext,
            IInvoicePdfGenerator pdfGenerator,   // ← NEW
            IInvoiceBlobService invoiceBlob)
        {
            _clientRepo = clientRepo;
            _matterRepo = matterRepo;
            _timeRepo = timeRepo;
            _expenseRepo = expenseRepo;
            _invoiceRepo = invoiceRepo;
            _user = userContext;
            _pdfGenerator = pdfGenerator;           // ← NEW
            _invoiceBlob = invoiceBlob;
        }

        public async Task<string> Handle(GenerateInvoiceFromSelectionCommand request,
                                         CancellationToken ct)
        {
            // 1. Verify client
            var client = await _clientRepo.GetByNicAsync(request.ClientNic)
                         ?? throw new Exception("Client not found");

            // 2. Load entries
            var allTimeEntries = await _timeRepo.GetAllEntriesByIdsAsync(request.EntryIds, request.UserId);
            var allExpenseEntries = await _expenseRepo.GetAllEntriesByIdsAsync(request.EntryIds, request.UserId);

            // 3. Duplicate guard
            if (!request.Force &&
                (allTimeEntries.Any(t => t.InvoiceId != null) ||
                 allExpenseEntries.Any(e => e.InvoiceId != null)))
                throw new InvalidOperationException("One or more selected entries already belong to an invoice.");

            // 4. Load matters
            var matterIds = allTimeEntries.Select(t => t.matterId)
                                          .Concat(allExpenseEntries.Select(e => e.matterId))
                                          .Distinct()
                                          .ToList();
            var matters = await _matterRepo.GetMattersByIdsAsync(matterIds, request.UserId);

            // 5. Totals
            var totalAmount = allTimeEntries.Sum(t => t.Amount) +
                              allExpenseEntries.Sum(e => e.Amount);
            var firstMatter = matters.FirstOrDefault();

            // 6. Build invoice (id first so blob path is known)
            var invoice = new Invoice
            {
                id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                ClientId = client.id!,
                ClientNic = client.nic!,
                ClientName = $"{client.FirstName} {client.LastName}".Trim(),
                ClientAddress = client.Address ?? string.Empty,
                MatterIds = matters.Select(m => m.id!).ToList(),
                MatterTitles = matters.Select(m => m.title!).ToList(),
                EntryIds = allTimeEntries.Select(t => t.id!)
                                              .Concat(allExpenseEntries.Select(e => e.id!))
                                              .ToList(),
                InvoiceNumber = await GenerateNextInvoiceNumberAsync(request.UserId),
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                TotalAmount = totalAmount,
                PaidAmount = 0,
                Status = "Draft",
                FooterNotes = $"Please make all amounts payable to: Law Office of {_user.FullName}",
                CreatedBy = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Subject = firstMatter?.title ?? string.Empty,   // 🆕
                Currency = "Rs.",
            };

            // 7. Generate PDF and upload
            var pdfBytes = await _pdfGenerator.GeneratePdfAsync(
                               invoice, allTimeEntries, allExpenseEntries);

            invoice.PdfFileUrl = await _invoiceBlob
                .SavePdfAsync(request.UserId, invoice.id, pdfBytes);

            // 8. Persist invoice (single write)
            await _invoiceRepo.CreateAsync(invoice);

            // 9. Mark entries as invoiced
            await _timeRepo.MarkEntriesInvoicedAsync(
                    allTimeEntries.Select(t => t.id), invoice.id, request.UserId);

            await _expenseRepo.MarkEntriesInvoicedAsync(
                    allExpenseEntries.Select(e => e.id), invoice.id, request.UserId);

            return invoice.id;
        }

        private async Task<int> GenerateNextInvoiceNumberAsync(string userId)
        {
            var last = await _invoiceRepo.GetLastInvoiceAsync(userId);
            return (last?.InvoiceNumber ?? 0) + 1;
        }
    }
}
