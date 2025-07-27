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
    public class UpdateTokenHandler : IRequestHandler<UpdateTokenCommand, bool>
    {
        private readonly ITokenRepository _tokenRepository;

        public UpdateTokenHandler(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<bool> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenEntry = await _tokenRepository.GetTokenAsync(request.Token);
            if (tokenEntry == null)
                return false;

            // Update fields if provided
            if (request.Otp != null)
                tokenEntry.Otp = request.Otp;

            if (request.IsUsed.HasValue)
                tokenEntry.IsUsed = request.IsUsed.Value;

            if (request.ExpiresAt.HasValue)
                tokenEntry.ExpiresAt = request.ExpiresAt.Value;

            await _tokenRepository.UpdateTokenAsync(tokenEntry);
            return true;
        }
    }
}