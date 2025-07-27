using MediatR;

namespace Aipazz.Application.client.Commands
{
    public record RemoveClientFromTeamCommand(string ClientId, string UserId) : IRequest<bool>;
}