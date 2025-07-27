using MediatR;

namespace Aipazz.Application.client.Commands
{
    public record ShareClientToTeamCommand(string ClientId, string TeamId, string UserId) : IRequest<Unit>;
}