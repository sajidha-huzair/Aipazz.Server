using System.Collections.Generic;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetAllTeamsQuery(string UserId) : IRequest<List<Aipazz.Domian.Team.Team>>;
}