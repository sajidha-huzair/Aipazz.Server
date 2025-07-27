using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Application.OtherDocuments.Queries;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class DownloadOtherDocumentHandler : IRequestHandler<DownloadOtherDocumentQuery, (byte[] content, string contentType, string fileName)?>
    {
        private readonly IOtherDocumentRepository _repository;
        private readonly IOtherDocumentStorageService _storageService;

        public DownloadOtherDocumentHandler(IOtherDocumentRepository repository, IOtherDocumentStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        public async Task<(byte[] content, string contentType, string fileName)?> Handle(DownloadOtherDocumentQuery request, CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                return null;

            return await _storageService.GetFileAsync(document.FileUrl);
        }
    }
}