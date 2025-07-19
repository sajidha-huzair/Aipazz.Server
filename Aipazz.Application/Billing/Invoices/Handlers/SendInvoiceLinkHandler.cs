using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Common.Aipazz.Application.Common;
using Aipazz.Application.Matters.matter.Commands.Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class SendInvoiceLinkHandler : IRequestHandler<SendInvoiceLinkCommand, SendLinkResult>
    {
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly ITokenRepository _tokenRepo;
        private readonly IEmailService _emailService;
        private readonly IUserContext _userContext;
        public SendInvoiceLinkHandler(
            IInvoiceRepository invoiceRepo,
            ITokenRepository tokenRepo,
            IEmailService emailService,
            IUserContext userContext)
        {
            _invoiceRepo = invoiceRepo;
            _tokenRepo = tokenRepo;
            _emailService = emailService;
            _userContext = userContext;
        }

        public async Task<SendLinkResult> Handle(SendInvoiceLinkCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(request.InvoiceId, request.UserId);
            if (invoice == null)
                return new SendLinkResult(false, "Invoice not found or unauthorized.");

            var token = Guid.NewGuid().ToString(); 
            var otp = new Random().Next(100000, 999999).ToString();
            var accessRecord = new InvoiceAccessToken
            {
                id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                InvoiceId = request.InvoiceId,
                RecipientEmail = request.RecipientEmail,
                Token = token,
                Otp= otp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(1),
                OtpVerified = false,
                IsUsed = false,
            };

            await _tokenRepo.SaveTokenAsync(accessRecord);

            var emailBody = EmailTemplateGenerator.GenerateInvoiceLinkEmail(invoice, token, _userContext);
            await _emailService.SendInvoiceAccessEmailAsync(request.RecipientEmail, "View Your Invoice", emailBody,request.SenderEmail);

            return new SendLinkResult(true, "Email sent.");
        }
    }

}
