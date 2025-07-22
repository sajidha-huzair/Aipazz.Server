using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;


namespace AIpazz.Infrastructure.Billing
{
    public class PayHerePaymentService : IPaymentService
    {
        private readonly IConfiguration _config;

        public PayHerePaymentService(IConfiguration config)
        {
            _config = config;
        }
        private string ComputeMd5(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return string.Concat(hashBytes.Select(b => b.ToString("X2")));
        }
        public Task<string> GeneratePaymentRedirectUrlAsync(StartPaymentRequest request)
        {
            var merchantId = _config["PayHere:MerchantId"] ;
            var merchantSecret = _config["PayHere:MerchantSecret"];
            var sandbox = _config["PayHere:UseSandbox"] == "true";
            var baseUrl = sandbox
                ? "https://sandbox.payhere.lk/pay/checkout"
                : "https://www.payhere.lk/pay/checkout";

            var notifyUrl = _config["PayHere:NotifyUrl"];
            var returnUrl = _config["PayHere:ReturnUrl"];
            var cancelUrl = _config["PayHere:CancelUrl"];

            var amountFormatted = request.Amount.ToString("0.00");
            var hashedSecret = ComputeMd5(merchantSecret).ToUpper();
            var hashInput = merchantId + request.InvoiceId + amountFormatted + request.Currency + hashedSecret;
            var hash = ComputeMd5(hashInput).ToUpper();

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["merchant_id"] = merchantId;
            queryParams["return_url"] = returnUrl;
            queryParams["cancel_url"] = cancelUrl;
            queryParams["notify_url"] = notifyUrl;

            queryParams["order_id"] = request.InvoiceId;
            queryParams["items"] = "Legal Invoice";
            queryParams["currency"] = request.Currency;
            queryParams["amount"] = amountFormatted;


            queryParams["first_name"] = request.FirstName;
            queryParams["last_name"] = request.LastName;
            queryParams["email"] = request.Email;
            queryParams["phone"] = request.Phone;
            queryParams["address"] = request.Address;
            queryParams["city"] = request.City;
            queryParams["country"] = request.Country;
            queryParams["hash"] = hash;

            // Optional: You can pass your UserId or extra info using custom fields
            queryParams["custom_1"] = request.UserId;

            var finalUrl = $"{baseUrl}?{queryParams.ToString()}";

            return Task.FromResult(finalUrl);
        }
    }
}
