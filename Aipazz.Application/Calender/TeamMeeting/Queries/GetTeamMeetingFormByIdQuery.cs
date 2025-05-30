using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Queries
{
    public record GetTeamMeetingFormByIdQuery(Guid Id) : IRequest<Domian.Calender.TeamMeetingForm?>;
}