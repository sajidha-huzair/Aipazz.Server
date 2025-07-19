using MediatR;

namespace Aipazz.Application.OtherDocuments.Commands
{
    public record ShareOtherDocumentToTeamCommand(string DocumentId, string TeamId, string UserId) : IRequest;
}