using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class SendInvoiceLinkRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string InvoiceId { get; set; }=string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
    }
}
