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
    public class VerifyInvoiceLinkHandler : IRequestHandler<VerifyInvoiceLinkCommand, VerifyLinkResult>
    {
        private readonly ITokenRepository _tokenRepository;

        public VerifyInvoiceLinkHandler(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<VerifyLinkResult> Handle(VerifyInvoiceLinkCommand request, CancellationToken cancellationToken)
        {
            var tokenData = await _tokenRepository.GetTokenAsync(request.Token);

            if (tokenData == null  || tokenData.ExpiresAt < DateTime.UtcNow)
            {
                return new VerifyLinkResult
                {
                    IsValid = false,
                    Message = "Invalid or expired link."
                };
            }

            if (tokenData.Otp != request.Otp)
            {
                return new VerifyLinkResult
                {
                    IsValid = false,
                    Message = "Incorrect OTP."
                };
            }

           
            return new VerifyLinkResult
            {
                IsValid = true,
                InvoiceId = tokenData.InvoiceId,
                Message = "OTP verified. Access granted."
            };
        }
    }
    }
