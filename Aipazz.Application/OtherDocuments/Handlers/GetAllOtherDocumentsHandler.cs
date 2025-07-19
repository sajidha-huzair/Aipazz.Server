using Aipazz.Application.OtherDocuments.DTOs;
using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Application.OtherDocuments.Queries;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class GetAllOtherDocumentsHandler : IRequestHandler<GetAllOtherDocumentsQuery, IEnumerable<OtherDocumentResponse>>
    {
        private readonly IOtherDocumentRepository _repository;

        public GetAllOtherDocumentsHandler(IOtherDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OtherDocumentResponse>> Handle(GetAllOtherDocumentsQuery request, CancellationToken cancellationToken)
        {
            var documents = await _repository.GetAllByUserIdAsync(request.UserId);

            return documents.Select(doc => new OtherDocumentResponse
            {
                Id = doc.id,
                FileName = doc.FileName,
                OriginalFileName = doc.OriginalFileName,
                ContentType = doc.ContentType,
                FileSize = doc.FileSize,
                UserName = doc.UserName,
                FileUrl = doc.FileUrl,
                MatterId = doc.MatterId,
                MatterName = doc.MatterName,
                CreatedAt = doc.CreatedAt,
                LastModifiedAt = doc.LastModifiedAt
            });
        }
    }
}