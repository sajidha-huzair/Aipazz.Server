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
    public class SendOtpHandler : IRequestHandler<SendOtpCommand, bool>
    {
        private readonly ITokenRepository _tokenRepo;
        private readonly IEmailService _emailService;

        public SendOtpHandler(ITokenRepository tokenRepo, IEmailService emailService)
        {
            _tokenRepo = tokenRepo;
            _emailService = emailService;
        }

        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var tokenData = await _tokenRepo.GetTokenAsync(request.Token);

            if (tokenData == null || tokenData.ExpiresAt < DateTime.UtcNow)
                return false;

            var otp = new Random().Next(100000, 999999).ToString();

            tokenData.Otp = otp;
            tokenData.ExpiresAt = DateTime.UtcNow.AddMinutes(20);
            tokenData.OtpVerified = false;

            await _tokenRepo.SaveTokenAsync(tokenData);

            var emailBody = $"<p>Your OTP for viewing the invoice is: <strong>{otp}</strong></p>";

            await _emailService.SendOtpEmailAsync(
                tokenData.RecipientEmail,
                "OTP for Invoice Access",
                emailBody
            );

            return true;
        }
    }
}
