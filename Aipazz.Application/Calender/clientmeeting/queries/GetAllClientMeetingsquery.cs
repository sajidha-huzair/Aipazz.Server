using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.queries;

public record GetAllClientMeetingsquery(string UserId):IRequest<List<ClientMeeting>>;