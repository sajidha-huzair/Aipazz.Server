using System.Collections.Generic;
using Aipazz.Application.Team.DTOs;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetTeamMembersQuery(string TeamId, string UserId) : IRequest<List<TeamMemberDto>>;
}