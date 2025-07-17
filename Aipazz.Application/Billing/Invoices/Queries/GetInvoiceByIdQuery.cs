using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Queries
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDetailsDto>
    {
        public string InvoiceId { get; set; }
        public string UserId { get; set; }

        public GetInvoiceByIdQuery(string invoiceId, string userId)
        {
            InvoiceId = invoiceId;
            UserId = userId;
        }
    }

}
