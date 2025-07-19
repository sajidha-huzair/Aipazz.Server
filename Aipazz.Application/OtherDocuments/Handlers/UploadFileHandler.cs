using Aipazz.Application.OtherDocuments.Commands;
using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Domian.OtherDocuments;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly IOtherDocumentRepository _repository;
        private readonly IOtherDocumentStorageService _storageService;

        public UploadFileHandler(IOtherDocumentRepository repository, IOtherDocumentStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        public async Task<string> Handle(UploadFileCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var documentId = Guid.NewGuid().ToString();

            // Validate file type (PDF and images)
            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };
            if (!allowedTypes.Contains(request.File.ContentType.ToLower()))
            {
                throw new ArgumentException("Only PDF and image files are allowed.");
            }

            // Validate file size (e.g., 10MB limit)
            const long maxFileSize = 10 * 1024 * 1024; // 10MB
            if (request.File.Length > maxFileSize)
            {
                throw new ArgumentException("File size cannot exceed 10MB.");
            }

            var fileName = $"{documentId}_{request.File.FileName}";
            var fileUrl = await _storageService.SaveFileAsync(command.UserId, documentId, fileName, request.File);

            var document = new OtherDocument
            {
                id = documentId,  // Changed from Id to id
                FileName = fileName,
                OriginalFileName = request.File.FileName,
                ContentType = request.File.ContentType,
                FileSize = request.File.Length,
                UserId = command.UserId,
                UserName = command.UserName,
                FileUrl = fileUrl,
                MatterId = request.MatterId,
                MatterName = request.MatterName
            };

            await _repository.SaveAsync(document);
            return documentId;
        }
    }
}