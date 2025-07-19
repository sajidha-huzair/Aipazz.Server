using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    public record RemoveDocumentFromTeamCommand(string DocumentId, string UserId) : IRequest<bool>;
}