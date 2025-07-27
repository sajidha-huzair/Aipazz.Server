using Aipazz.Application.OtherDocuments.DTOs;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Queries
{
    public record GetOtherDocumentsByMatterIdQuery(string MatterId, string UserId) : IRequest<IEnumerable<OtherDocumentResponse>>;
}