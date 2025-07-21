using Aipazz.Application.OtherDocuments.DTOs;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Queries
{
    public record GetAllOtherDocumentsQuery(string UserId) : IRequest<IEnumerable<OtherDocumentResponse>>;
}