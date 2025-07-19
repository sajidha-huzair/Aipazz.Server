using MediatR;

namespace Aipazz.Application.Matters.matter.Commands
{
    public record ShareMatterToTeamCommand(string MatterId, string ClientNic, string TeamId, string UserId) : IRequest<bool>;
}