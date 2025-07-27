using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class VerifyLinkResult
    {
        public bool IsValid { get; set; }
        public string? InvoiceId { get; set; }
        public string? Message { get; set; }
    }

}
