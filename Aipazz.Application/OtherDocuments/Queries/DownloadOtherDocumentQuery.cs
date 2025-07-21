using MediatR;

namespace Aipazz.Application.OtherDocuments.Queries
{
    public record DownloadOtherDocumentQuery(string DocumentId, string UserId) : IRequest<(byte[] content, string contentType, string fileName)?>;
}