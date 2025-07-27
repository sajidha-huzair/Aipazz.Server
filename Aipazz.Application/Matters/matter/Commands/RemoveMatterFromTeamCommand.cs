using MediatR;

namespace Aipazz.Application.Matters.matter.Commands
{
    public record RemoveMatterFromTeamCommand(string MatterId, string ClientNic, string UserId) : IRequest<bool>;
}