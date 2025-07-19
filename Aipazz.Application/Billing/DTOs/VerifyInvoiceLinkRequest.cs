using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class VerifyInvoiceLinkRequest
    {
        public string Token { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }

}
