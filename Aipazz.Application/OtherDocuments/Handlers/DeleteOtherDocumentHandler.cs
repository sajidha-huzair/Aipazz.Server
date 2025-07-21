using Aipazz.Application.OtherDocuments.Commands;
using Aipazz.Application.OtherDocuments.Interfaces;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class DeleteOtherDocumentHandler : IRequestHandler<DeleteOtherDocumentCommand, bool>
    {
        private readonly IOtherDocumentRepository _repository;
        private readonly IOtherDocumentStorageService _storageService;

        public DeleteOtherDocumentHandler(IOtherDocumentRepository repository, IOtherDocumentStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        public async Task<bool> Handle(DeleteOtherDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                return false;

            // Delete file from storage
            await _storageService.DeleteFileAsync(document.FileUrl);

            // Delete from database
            return await _repository.DeleteAsync(request.DocumentId, request.UserId);
        }
    }
}