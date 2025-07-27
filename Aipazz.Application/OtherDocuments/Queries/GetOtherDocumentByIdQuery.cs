using Aipazz.Application.OtherDocuments.DTOs;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Queries
{
    public record GetOtherDocumentByIdQuery(string DocumentId, string UserId) : IRequest<OtherDocumentResponse?>;
}