using Aipazz.Application.Team.DTOs;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetTeamMemberByIdQuery(string TeamId, string MemberId, string UserId) : IRequest<TeamMemberDto?>;
}