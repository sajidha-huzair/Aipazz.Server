using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetTeamByIdQuery(string TeamId, string UserId) : IRequest<Aipazz.Domian.Team.Team?>;
}