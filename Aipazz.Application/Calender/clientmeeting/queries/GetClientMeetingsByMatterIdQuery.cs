using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.clientmeeting.Queries
{
    public record GetClientMeetingsByMatterIdQuery(string MatterId, string UserId) : IRequest<List<ClientMeeting>>;

}
