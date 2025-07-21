using MediatR;

namespace Aipazz.Application.OtherDocuments.Commands
{
    public record RemoveOtherDocumentFromTeamCommand(string DocumentId, string UserId) : IRequest<bool>;
}