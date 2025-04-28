using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.clientmeeting.Commands
{
    public record CreateClientMeetingCommand(
        string Title,
        DateOnly Date,
        TimeOnly Time,
        bool Repeat,
        TimeSpan? Reminder,
        string? Description,
        string? MeetingLink,
        string? Location,
        List<string> TeamMembers,
        string ClientEmail
    ) : IRequest<ClientMeeting>;
}