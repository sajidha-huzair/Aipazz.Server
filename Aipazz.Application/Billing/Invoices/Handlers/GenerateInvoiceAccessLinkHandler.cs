using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class GenerateInvoiceAccessLinkHandler : IRequestHandler<GenerateInvoiceAccessLinkCommand, string>
    {
        private readonly ITokenRepository _tokenRepository;

        public GenerateInvoiceAccessLinkHandler(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<string> Handle(GenerateInvoiceAccessLinkCommand request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid().ToString();
            var otp = new Random().Next(100000, 999999).ToString();

            var tokenEntity = new InvoiceAccessToken
            {
                id = Guid.NewGuid().ToString(),
                Token = token,
                Otp = otp,
                InvoiceId = request.InvoiceId,
                UserId = request.UserId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _tokenRepository.SaveTokenAsync(tokenEntity);

            // Log OTP or return it along with the link for debugging/dev purposes
            return $"http://localhost:5173/view-invoice?token={token}";
        }
    }

}
