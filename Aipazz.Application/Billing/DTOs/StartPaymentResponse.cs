using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    // Application/Billing/DTOs/StartPaymentResponse.cs
    public class StartPaymentResponse
    {
        public string? ClientSecret { get; set; }
        public string? RedirectUrl { get; set; }
    }

}
