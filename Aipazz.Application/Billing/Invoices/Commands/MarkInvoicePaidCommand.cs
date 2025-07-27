using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class MarkInvoicePaidCommand : IRequest<bool>
    {
        public string InvoiceId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal PaidAmount { get; set; } 
        public string UserId { get; set; } = string.Empty; // optional
    }

}
