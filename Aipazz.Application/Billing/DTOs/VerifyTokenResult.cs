using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class VerifyTokenResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? InvoiceId { get; set; }

        public static VerifyTokenResult Fail(string message) =>
            new() { Success = false, Message = message };

        public static VerifyTokenResult Ok(string invoiceId) =>
            new() { Success = true, InvoiceId = invoiceId };
    }

}
