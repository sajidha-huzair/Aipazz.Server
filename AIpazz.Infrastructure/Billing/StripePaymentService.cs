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
using Stripe.Checkout;
using Stripe;


namespace AIpazz.Infrastructure.Billing
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _config;

        public StripePaymentService(IConfiguration config)
        {
            _config = config;
            Stripe.StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public Task<string> GeneratePaymentRedirectUrlAsync(StartPaymentRequest request)
        {
            var domain = _config["Stripe:Domain"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(request.Amount * 100),
                        Currency = request.Currency.ToLower(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Legal Invoice Payment",
                            Description = $"Invoice #{request.InvoiceId}"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = $"{domain}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/payment-cancel",
                Metadata = new Dictionary<string, string>
            {
                { "InvoiceId", request.InvoiceId },
                { "UserId", request.UserId }
            }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Task.FromResult(session.Url);
        }

        public async Task<string> CreatePaymentIntentAsync(StartPaymentRequest request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency.ToLower(),
                Metadata = new Dictionary<string, string>
            {
                { "InvoiceId", request.InvoiceId },
                { "UserId", request.UserId }
            },
                Description = $"Invoice #{request.InvoiceId}",
                ReceiptEmail = request.Email
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return intent.ClientSecret;
        }
    }

}
