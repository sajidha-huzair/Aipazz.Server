using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using MediatR;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class DeleteInvoiceHandler : IRequestHandler<DeleteInvoiceCommand, Unit>
    {
        private readonly IInvoiceRepository _repo;
        private readonly IInvoiceBlobService _blob;

        public DeleteInvoiceHandler(
            IInvoiceRepository repo,
            IInvoiceBlobService blob)
        {
            _repo = repo;
            _blob = blob;
        }

        public async Task<Unit> Handle(DeleteInvoiceCommand request,
                                       CancellationToken ct)
        {
            // 1. Fetch the invoice to get the PDF URL
            var invoice = await _repo.GetByIdAsync(request.InvoiceId, request.UserId);
            if (invoice == null)
                throw new Exception("Invoice not found");

            // 2. Delete the PDF blob (ignore result if URL missing)
            if (!string.IsNullOrWhiteSpace(invoice.PdfFileUrl))
                await _blob.DeletePdfAsync(invoice.PdfFileUrl);

            // 3. Delete the invoice document
            await _repo.DeleteAsync(request.InvoiceId, request.UserId);

            return Unit.Value;
        }
    }
}
