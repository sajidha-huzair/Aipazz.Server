using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Commands
{
    public record DeleteClientMeetingCommand(Guid Id) : IRequest<bool>;
}