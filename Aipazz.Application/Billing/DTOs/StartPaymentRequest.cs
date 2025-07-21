using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class StartPaymentRequest
    {
        public string InvoiceId { get; set; }=string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "LKR";
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = "Sri Lanka";
        public string UserId { get; set; } = string.Empty; // for notification after payment
    }
}
