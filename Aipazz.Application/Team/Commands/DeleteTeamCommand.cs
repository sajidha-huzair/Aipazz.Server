using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record DeleteTeamCommand(string TeamId, string OwnerId) : IRequest<Unit>;
}