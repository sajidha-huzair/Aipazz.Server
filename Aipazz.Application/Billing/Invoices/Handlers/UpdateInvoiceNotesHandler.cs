using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Common.Aipazz.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class UpdateInvoiceNotesHandler
     : IRequestHandler<UpdateInvoiceNotesCommand, string>
    {
        private readonly IInvoiceRepository _repo;
        private readonly IInvoicePdfGenerator _pdf;
        private readonly IInvoiceBlobService _blob;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;
        private readonly IUserContext _user;   // for {UserName}

        public UpdateInvoiceNotesHandler(
            IInvoiceRepository repo,
            IInvoicePdfGenerator pdf,
            IInvoiceBlobService blob,
            ITimeEntryRepository timeRepo,
            IExpenseEntryRepository expenseRepo,
            IUserContext user)
        {
            _repo = repo;
            _pdf = pdf;
            _blob = blob;
            _timeRepo = timeRepo;
            _expenseRepo = expenseRepo;
            _user = user;
        }

        public async Task<string> Handle(UpdateInvoiceNotesCommand cmd, CancellationToken ct)
        {
            var invoice = await _repo.GetByIdAsync(cmd.InvoiceId, cmd.UserId)
                         ?? throw new Exception("Invoice not found");

            // ─── Replace the footer outright ───
            invoice.FooterNotes = cmd.FooterNotes?.Trim() ?? string.Empty;

            invoice.PaymentProfileNotes = cmd.PaymentProfileNotes;
            invoice.UpdatedBy = cmd.UserId;
            invoice.UpdatedAt = DateTime.UtcNow;

            // reload entries only to render PDF
            var time = await _timeRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, cmd.UserId);
            var expense = await _expenseRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, cmd.UserId);

            // delete old file
            if (!string.IsNullOrWhiteSpace(invoice.PdfFileUrl))
                await _blob.DeletePdfAsync(invoice.PdfFileUrl);

            // regenerate + upload
            var pdfBytes = await _pdf.GeneratePdfAsync(invoice, time, expense);
            invoice.PdfFileUrl = await _blob.SavePdfAsync(cmd.UserId, invoice.id, pdfBytes);

            await _repo.UpdateAsync(invoice);

            return invoice.PdfFileUrl;
        }

    }

}
