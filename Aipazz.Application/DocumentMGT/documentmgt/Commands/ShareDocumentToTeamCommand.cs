using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    public record ShareDocumentToTeamCommand(string DocumentId, string TeamId, string UserId) : IRequest<Unit>;
}