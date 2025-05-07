using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.queries
{
    public record GetClientMeetingByIdQuery(Guid Id) : IRequest<ClientMeeting>;
}