using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record UpdateTeamCommand(Aipazz.Domian.Team.Team Team) : IRequest<Unit>;
}