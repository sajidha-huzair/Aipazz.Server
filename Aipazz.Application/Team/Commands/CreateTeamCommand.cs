using System.Collections.Generic;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Commands
{
    public record CreateTeamCommand(
        string Name,
        string Description,
        string UserId,
        string OwnerName, // Add this
        string? UserEmail, // Add this
        List<TeamMember> Members
    ) : IRequest<string>;
}