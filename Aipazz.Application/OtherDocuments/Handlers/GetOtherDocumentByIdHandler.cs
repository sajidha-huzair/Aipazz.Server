using Aipazz.Application.OtherDocuments.DTOs;
using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Application.OtherDocuments.Queries;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class GetOtherDocumentByIdHandler : IRequestHandler<GetOtherDocumentByIdQuery, OtherDocumentResponse?>
    {
        private readonly IOtherDocumentRepository _repository;

        public GetOtherDocumentByIdHandler(IOtherDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<OtherDocumentResponse?> Handle(GetOtherDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                return null;

            return new OtherDocumentResponse
            {
                Id = document.id,
                FileName = document.FileName,
                OriginalFileName = document.OriginalFileName,
                ContentType = document.ContentType,
                FileSize = document.FileSize,
                UserName = document.UserName,
                FileUrl = document.FileUrl,
                MatterId = document.MatterId,
                MatterName = document.MatterName,
                CreatedAt = document.CreatedAt,
                LastModifiedAt = document.LastModifiedAt
            };
        }
    }
}