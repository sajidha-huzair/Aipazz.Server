using System.Net;
using System.Net.Mail;
using Aipazz.Application.Calender.Interface;
using Microsoft.Extensions.Configuration;

namespace AIpazz.Infrastructure.Calender;

public class CalenderEmailService : IEmailService
{
    private readonly string _email;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;

    public CalenderEmailService(IConfiguration configuration)
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
}