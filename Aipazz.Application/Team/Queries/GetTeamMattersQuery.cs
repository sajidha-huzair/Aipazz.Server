using System.Collections.Generic;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetTeamMattersQuery(string TeamId) : IRequest<List<Matter>>;
}