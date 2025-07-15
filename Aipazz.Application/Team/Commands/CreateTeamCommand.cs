using System.Collections.Generic;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record CreateTeamCommand(
        string Name,
        string Description,
        string UserId, // Add this
        List<TeamMember> Members
    ) : IRequest<string>;
}