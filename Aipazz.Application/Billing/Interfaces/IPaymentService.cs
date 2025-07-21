using Aipazz.Application.Billing.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    // Application/Billing/Interfaces/IPaymentService.cs
    public interface IPaymentService
    {
        Task<string> GeneratePaymentRedirectUrlAsync(StartPaymentRequest request);
    }

}
