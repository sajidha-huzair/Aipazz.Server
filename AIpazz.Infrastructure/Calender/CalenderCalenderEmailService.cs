using System.Net;
using System.Net.Mail;
using Aipazz.Application.Calender.Interface;
using Microsoft.Extensions.Configuration;

namespace AIpazz.Infrastructure.Calender;

public class CalenderCalenderEmailService : ICalenderEmailService
{
    private readonly string _email;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;

    public CalenderCalenderEmailService(IConfiguration configuration)
    {
        _email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
        _password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
        _host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
        _port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");
    }

    public async Task sendEmaiToClient( string recipients, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var recipient in recipients)
            {
                if (!string.IsNullOrWhiteSpace(recipients))
                    mailMessage.To.Add(recipients);
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log or handle the error as needed
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }
    
    public async Task SendCourtDateEmailToClientAsync(
        string clientEmail,
        string title,
        string? courtType,
        string? stage,
        DateTime courtDate,
        TimeSpan reminder,
        string? note)
    {
        try
        {
            var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = true,
            };

            string formattedDate = courtDate.ToString("dddd, MMMM dd, yyyy");
            string formattedTime = courtDate.ToString("hh:mm tt");

            string subject = $"Court Date Scheduled: {title}";

            string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                <div style='background-color: #fff; padding: 20px; border-radius: 8px; max-width: 600px; margin: auto; box-shadow: 0 2px 6px rgba(0,0,0,0.1);'>
                    <h2 style='color: #2b7a78;'>Court Date Notification</h2>
                    <p><strong>Title:</strong> {title}</p>
                    {(string.IsNullOrWhiteSpace(courtType) ? "" : $"<p><strong>Court Type:</strong> {courtType}</p>")}
                    {(string.IsNullOrWhiteSpace(stage) ? "" : $"<p><strong>Stage:</strong> {stage}</p>")}
                    <p><strong>Date:</strong> {formattedDate}</p>
                    <p><strong>Time:</strong> {formattedTime}</p>
                    <p><strong>Reminder:</strong> {reminder.TotalDays} day(s) before</p>
                    {(string.IsNullOrWhiteSpace(note) ? "" : $"<p><strong>Note:</strong> {note}</p>")}
                    <p>Kindly check your Aipazz dashboard for more details.</p>
                    <a href='https://witty-field-0e9483e0f.6.azurestaticapps.net/' style='display:inline-block;margin-top:20px;padding:10px 15px;background-color:#2b7a78;color:#fff;text-decoration:none;border-radius:6px;'>Open Dashboard</a>
                </div>
            </body>
            </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(clientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending court date email: {ex.Message}");
            throw;
        }
    }
}