using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class VerifyTokenHandler : IRequestHandler<VerifyTokenCommand, VerifyTokenResult>
    {
        private readonly ITokenRepository _tokenRepository;

        public VerifyTokenHandler(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<VerifyTokenResult> Handle(VerifyTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenEntry = await _tokenRepository.GetTokenAsync(request.Token);

            if (tokenEntry == null)
                return VerifyTokenResult.Fail("Invalid or expired token.");

            if (tokenEntry.ExpiresAt < DateTime.UtcNow)
            {
                return VerifyTokenResult.Fail("Token has expired.");
            }

            return VerifyTokenResult.Ok(tokenEntry.InvoiceId);
        }
    }
}
