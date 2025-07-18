using Aipazz.Application.Team.DTOs;
using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record UpdateTeamMemberCommand(string TeamId, string MemberId, string UserId, UpdateTeamMemberDto Member) : IRequest<Unit>;
}