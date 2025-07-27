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
    public class GetAllInvoicesForUserHandler : IRequestHandler<GetAllInvoicesForUserQuery, List<InvoiceListDto>>
    {
        private readonly IInvoiceRepository _invoiceRepo;

        public GetAllInvoicesForUserHandler(IInvoiceRepository invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        public async Task<List<InvoiceListDto>> Handle(GetAllInvoicesForUserQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepo.GetAllForUserAsync(request.UserId);

            return invoices.Select(invoice => new InvoiceListDto
            {
                Id = invoice.id,
                ClientName = invoice.ClientName,
                MatterSummary = string.Join(", ", invoice.MatterTitles),
                IssueDate = invoice.IssueDate,
                DaysUntilDue = (invoice.DueDate - DateTime.UtcNow).Days,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status
            }).ToList();
        }
    }

}
