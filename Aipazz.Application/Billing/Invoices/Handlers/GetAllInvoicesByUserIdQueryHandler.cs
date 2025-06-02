using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Invoices.Queries;
using Aipazz.Application.Billing.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    /// <summary>
    /// Handles the retrieval of all invoices belonging to a specific user.
    /// Maps them to a summary DTO list for presentation.
    /// </summary>
    public class GetAllInvoicesByUserIdQueryHandler : IRequestHandler<GetAllInvoicesByUserIdQuery, List<InvoiceListDto>>
    {
        private readonly IInvoiceRepository _invoiceRepo;

        public GetAllInvoicesByUserIdQueryHandler(IInvoiceRepository invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        public async Task<List<InvoiceListDto>> Handle(GetAllInvoicesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepo.GetAllInvoicesByUserId(request.UserId);

            return invoices.Select(i => new InvoiceListDto
            {
                Id = i.Id,
                ClientName = i.ClientName,
                MatterSummary = string.Join(", ", i.MatterTitles),
                IssueDate = i.IssueDate,
                DaysUntilDue = (i.DueDate - DateTime.UtcNow).Days,
                TotalAmount = i.TotalAmount,
                IsSent = i.Status.ToLower() != "draft" // or customize for your statuses
            }).ToList();
        }
    }
}
