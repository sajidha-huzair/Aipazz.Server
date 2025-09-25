
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
            var apiKey = ""; // Use IConfiguration in prod
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
            var apiKey = ""; // move to config in prod
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
            string apiKey = ""; 
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
        
        public async Task SendClientMeetingEmailToMembersAsync(
            List<string> memberEmails,
            string meetingTitle,
            DateOnly meetingDate,
            TimeOnly meetingTime,
            string meetingLink,
            string ownerEmail)
        {
            string apiKey = "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";
            var client = new SendGridClient(apiKey);

            foreach (var memberEmail in memberEmails)
            {
                var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Client Meeting Invitation</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f6f9fc;
                            padding: 20px;
                            color: #333;
                        }}
                        .container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
                            max-width: 600px;
                            margin: 0 auto;
                        }}
                        h2 {{
                            color: #2b7a78;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            padding: 10px 15px;
                            margin-top: 20px;
                            background-color: #2b7a78;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 6px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>You're Invited to a Client Meeting</h2>
                        <p><strong>Title:</strong> {meetingTitle}</p>
                        <p><strong>Date:</strong> {meetingDate:dddd, MMMM dd, yyyy}</p>
                        <p><strong>Time:</strong> {meetingTime}</p>
                        {(string.IsNullOrWhiteSpace(meetingLink) ? "" : $"<p><strong>Meeting Link:</strong> <a href='{meetingLink}'>{meetingLink}</a></p>")}
                        <p>This meeting has been organized by your team. Please attend it on time.</p>
                        <p>Contact the meeting organizer at: <strong>{ownerEmail}</strong></p>
                        <a href='https://witty-field-0e9483e0f.6.azurestaticapps.net/' class='button'>Open Aipazz Dashboard</a>
                    </div>
                </body>
                </html>";

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal"),
                    Subject = $"Client Meeting: {meetingTitle}",
                    HtmlContent = htmlContent
                };
                msg.AddTo(new EmailAddress(memberEmail));

                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid failed for {memberEmail}: {response.StatusCode} - {error}");
                }
            }
        }
        
        public async Task SendCourtDateEmailToMembersAsync(
            List<string> teamMemberEmails,
            string title,
            string courtType,
            string stage,
            DateTime courtDate,
            DateTime reminder,
            string? note,
            string ownerEmail)
        {
            string apiKey = "";
            var client = new SendGridClient(apiKey);

            string formattedDate = courtDate.ToString("dddd, MMMM dd, yyyy");
            string formattedTime = courtDate.ToString("hh:mm tt");
            string reminderDays = $"{reminder}";

            foreach (var memberEmail in teamMemberEmails)
            {
                var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Court Date Notification</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f6f9fc;
                            padding: 20px;
                            color: #333;
                        }}
                        .container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
                            max-width: 600px;
                            margin: 0 auto;
                        }}
                        h2 {{
                            color: #2b7a78;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.5;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 15px;
                            margin-top: 20px;
                            background-color: #2b7a78;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 6px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>New Court Date Assigned</h2>
                        <p>Dear Team Member,</p>

                        <p>You have been assigned to a new court session. Please find the details below:</p>

                        <p><strong>Title:</strong> {title}</p>
                        <p><strong>Court Type:</strong> {courtType}</p>
                        <p><strong>Stage:</strong> {stage}</p>
                        <p><strong>Date:</strong> {formattedDate}</p>
                        <p><strong>Time:</strong> {formattedTime}</p>
                        {(string.IsNullOrWhiteSpace(note) ? "" : $"<p><strong>Note:</strong> {note}</p>")}

                        <p>Please be prepared and make arrangements accordingly.</p>
                        <p>For any clarification, contact the organizer: <strong>{ownerEmail}</strong></p>

                        <a href='https://witty-field-0e9483e0f.6.azurestaticapps.net/' class='button'>Open Aipazz Dashboard</a>
                    </div>
                </body>
                </html>";

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal"),
                    Subject = $"Court Date Assigned: {title}",
                    HtmlContent = htmlContent
                };
                msg.AddTo(new EmailAddress(memberEmail));

                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid failed for {memberEmail}: {response.StatusCode} - {error}");
                }
            }
        }
        
        public async Task SendTeamMeetingEmailToMembersAsync(
            List<string> teamMemberEmails,
            string title,
            DateTime meetingDate,
            string meetingTime,
            string? description,
            string? videoConferencingLink,
            string? locationLink,
            string ownerEmail)
        {
            string apiKey = "";
            var client = new SendGridClient(apiKey);

            string formattedDate = meetingDate.ToString("dddd, MMMM dd, yyyy");

            foreach (var memberEmail in teamMemberEmails)
            {
                var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Team Meeting Notification</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f6f9fc;
                            padding: 20px;
                            color: #333;
                        }}
                        .container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
                            max-width: 600px;
                            margin: 0 auto;
                        }}
                        h2 {{
                            color: #2b7a78;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.5;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 15px;
                            margin-top: 20px;
                            background-color: #2b7a78;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 6px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>You've Been Assigned to a Team Meeting</h2>
                        <p><strong>Title:</strong> {title}</p>
                        <p><strong>Date:</strong> {formattedDate}</p>
                        <p><strong>Time:</strong> {meetingTime}</p>
                        {(string.IsNullOrWhiteSpace(description) ? "" : $"<p><strong>Description:</strong> {description}</p>")}
                        {(string.IsNullOrWhiteSpace(videoConferencingLink) ? "" : $"<p><strong>Video Link:</strong> <a href='{videoConferencingLink}' target='_blank'>{videoConferencingLink}</a></p>")}
                        {(string.IsNullOrWhiteSpace(locationLink) ? "" : $"<p><strong>Location:</strong> <a href='{locationLink}' target='_blank'>{locationLink}</a></p>")}

                        <p>This meeting has been scheduled for your team. Please check the calendar and be prepared.</p>
                        <p>Contact the organizer at: <strong>{ownerEmail}</strong></p>

                        <a href='https://witty-field-0e9483e0f.6.azurestaticapps.net/' class='button'>Open Aipazz Dashboard</a>
                    </div>
                </body>
                </html>";

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("sajidhamhf.22@uom.lk", "Aipazz Legal"),
                    Subject = $"📅 Team Meeting Scheduled: {title}",
                    HtmlContent = htmlContent
                };
                msg.AddTo(new EmailAddress(memberEmail));

                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid failed for {memberEmail}: {response.StatusCode} - {error}");
                }
            }
        }

    }
}
