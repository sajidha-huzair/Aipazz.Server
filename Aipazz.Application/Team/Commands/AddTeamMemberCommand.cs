using Aipazz.Application.Team.DTOs;
using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record AddTeamMemberCommand(string TeamId, string UserId, AddTeamMemberDto   Member) : IRequest<TeamMemberDto>;
}