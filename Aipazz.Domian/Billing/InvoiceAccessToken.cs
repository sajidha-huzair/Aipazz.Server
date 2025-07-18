using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Billing
{
    public class InvoiceAccessToken
    {
        public string id { get; set; }=string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string InvoiceId { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public bool OtpVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

}
