using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailsDto>
    {
        private readonly IInvoiceRepository _invoiceRepo;

        public GetInvoiceByIdHandler(IInvoiceRepository invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        public async Task<InvoiceDetailsDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(request.InvoiceId, request.UserId);
            if (invoice == null) throw new Exception("Invoice not found");

            return new InvoiceDetailsDto
            {
                Id = invoice.id,
                UserId = invoice.UserId,
                ClientId = invoice.ClientId,
                ClientNic = invoice.ClientNic,
                ClientName = invoice.ClientName,
                ClientAddress = invoice.ClientAddress,
                MatterIds = invoice.MatterIds,
                MatterTitles = invoice.MatterTitles,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                EntryIds = invoice.EntryIds,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                DueAmount = invoice.TotalAmount - invoice.PaidAmount,
                Status = invoice.Status,
                FooterNotes = invoice.FooterNotes,
                PaymentProfileNotes = invoice.PaymentProfileNotes,
                PdfFileUrl = invoice.PdfFileUrl,
                CreatedBy = invoice.CreatedBy,
                CreatedAt = invoice.CreatedAt,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedAt = invoice.UpdatedAt
            };
        }
    }

}
