using Aipazz.Application.Matters.matter.Commands.Aipazz.Application.Billing.Invoices.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class SendInvoiceLinkCommand : IRequest<SendLinkResult>
    {
        public string UserId { get; }
        public string InvoiceId { get; }
        public string RecipientEmail { get; }
        public string SenderEmail { get; set; }

        public SendInvoiceLinkCommand(string userId, string invoiceId, string recipientEmail, string senderEmail)
        {
            UserId = userId;
            InvoiceId = invoiceId;
            RecipientEmail = recipientEmail;
            SenderEmail = senderEmail;
        }
    }

}
