using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Commands
{
    public record UpdateClientMeetingCommand(
        Guid Id,
        string Title,
        DateOnly Date,
        TimeOnly Time,
        DateTime Reminder,
        string? Description,
        string? MeetingLink,
        string? Location,
        List<string> TeamMembers,
        List<string> ClientEmails
    ) : IRequest<ClientMeeting>;
}