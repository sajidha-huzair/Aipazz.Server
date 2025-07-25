
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

        public async Task SendEmailtoMembers( string teamName, string MemberName, string MemberEmail, string ownerEmail)
        {
            string toEmail = MemberEmail;
            string apiKey = "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"; 
            var client = new SendGridClient(apiKey);
            var htmlContent = $@"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <title>Welcome to Your New Team</title>
    <style>
      body {{
        font-family: Arial, sans-serif;
        background-color: #f6f9fc;
        margin: 0;
        padding: 0;
        color: #333;
      }}
      .email-container {{
        max-width: 600px;
        margin: 40px auto;
        background-color: #ffffff;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        padding: 30px;
      }}
      .header {{
        border-bottom: 1px solid #eaeaea;
        padding-bottom: 20px;
        margin-bottom: 20px;
        text-align: center;
      }}
      .header h1 {{
        font-size: 24px;
        margin: 0;
        color: #2b7a78;
      }}
      .content p {{
        font-size: 16px;
        line-height: 1.6;
        margin: 16px 0;
      }}
      .button {{
        display: inline-block;
        padding: 12px 20px;
        background-color: #2b7a78;
        color: #FFFFFF;
        border-radius: 6px;
        text-decoration: none;
        margin-top: 20px;
      }}
      .footer {{
        font-size: 12px;
        color: #999;
        text-align: center;
        margin-top: 30px;
      }}
    </style>
  </head>
  <body>
    <div class=""email-container"">
      <div class=""header"">
        <h1>You've Joined a New Team!</h1>
      </div>
      <div class=""content"">
        <p>Hi <strong>{MemberName}</strong>,</p>

        <p>
          You have been successfully added as the owner of the team
          <strong>{teamName}</strong> on <strong>Aipazz</strong>.
        </p>

        <p>
          As a team member, you now have access to view shared documents, clients, and matter
          information — all in one place.
        </p>

        <p>We’re excited to have you on board!</p>

        <a href=""https://witty-field-0e9483e0f.6.azurestaticapps.net/"" style=""display:inline-block;padding:12px 20px;background-color:#2b7a78;color:#ffffff;text-decoration:none;border-radius:6px;font-weight:bold;""  >Go to Aipazz Dashboard</a>
      </div>

<p>
          If you have any question , Contact Team Owner through Email {ownerEmail}
        </p>

      <div class=""footer"">
        &copy; 2025 Aipazz. All rights reserved.
      </div>
    </div>
  </body>
</html>




";
            var msg = new SendGridMessage
            {
                From = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal"),
                Subject = "You have been added as a team owner",
                HtmlContent = htmlContent


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
