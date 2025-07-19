using MediatR;

namespace Aipazz.Application.OtherDocuments.Commands
{
    public record DeleteOtherDocumentCommand(string DocumentId, string UserId) : IRequest<bool>;
}