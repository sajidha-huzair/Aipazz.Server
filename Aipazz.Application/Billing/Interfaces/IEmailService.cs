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

        Task SendEmailtoMembers(string teamName, string MemberName, string MemberEmail, string ownerEmail);

        Task SendClientMeetingEmailToMembersAsync(
            List<string> memberEmails,
            string meetingTitle,
            DateOnly meetingDate,
            TimeOnly meetingTime,
            string meetingLink,
            string ownerEmail
        );

        Task SendCourtDateEmailToMembersAsync(
            List<string> teamMemberEmails,
            string title,
            string courtType,
            string stage,
            DateTime courtDate,
            DateTime reminder,
            string? note,
            string ownerEmail);


        Task SendTeamMeetingEmailToMembersAsync(
            List<string> teamMemberEmails,
            string title,
            DateTime meetingDate,
            string meetingTime,
            string? description,
            string? videoConferencingLink,
            string? locationLink,
            string ownerEmail);

    };
        
}


