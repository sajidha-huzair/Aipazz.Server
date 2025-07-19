
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Billing.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AIpazz.Infrastructure.Billing
{
    public class EmailService : IEmailService
    {
        public async Task SendInvoiceAccessEmailAsync(string recipientEmail, string subject, string htmlBody, string replyToEmail)
        {
            var apiKey = "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"; // Use IConfiguration in prod
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal");
            var to = new EmailAddress(recipientEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlBody);

            msg.ReplyTo = new EmailAddress(replyToEmail);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid failed: {response.StatusCode} - {error}");
            }
        }

        public async Task SendOtpEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var apiKey = "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"; // move to config in prod
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal"),
                Subject = subject,
                HtmlContent = htmlBody
            };
            msg.AddTo(new EmailAddress(toEmail));

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid failed: {response.StatusCode} - {error}");
            }
        }



    }
}
