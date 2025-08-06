using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.clientmeeting.Commands
{
    public record CreateClientMeetingCommand(
        Guid Id,
        string Title,
        DateOnly Date,
        TimeOnly Time,
        DateTime Reminder,
        string? Description,
        string? MeetingLink,
        string? Location,
        List<string> TeamMembers,
        List<string> TeamMemberEmails, // this is for client names
        List<string> ClientEmails, // this is for client emails
        string matterId
    ) : IRequest<ClientMeeting>
    {
        public string? UserId { get; set; }
    
    }
}