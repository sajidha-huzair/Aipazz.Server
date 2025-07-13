using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class DeleteInvoiceCommand : IRequest<Unit>
    {
        public string InvoiceId { get; }
        public string UserId { get; }

        public DeleteInvoiceCommand(string invoiceId, string userId)
        {
            InvoiceId = invoiceId;
            UserId = userId;
        }
    }
}
