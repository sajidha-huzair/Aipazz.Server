using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record DeleteTeamMemberCommand(string TeamId, string MemberId, string UserId) : IRequest<Unit>;
}