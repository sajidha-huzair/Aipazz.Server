using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
        public interface IEmailService
        {
            Task SendInvoiceAccessEmailAsync(string toEmail, string subject, string htmlContent, string replyToEmail);
            Task SendOtpEmailAsync(string toEmail, string subject, string htmlBody);

        Task SendEmailtoMembers( string teamName, string MemberName , string MemberEmail);

    }
}


