using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Handler
{
    public class DeleteUdtemplateHandler : IRequestHandler<DeleteUdtemplateCommand, Unit>
    {
        private readonly IUdtemplateRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public DeleteUdtemplateHandler(IUdtemplateRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(DeleteUdtemplateCommand request, CancellationToken cancellationToken)
        {
            // First, get the template to retrieve the file URL
            var template = await _repository.GetTemplateByIdAndUserId(request.Id, request.UserId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {request.Id} not found for user {request.UserId}.");
            }

            // Delete the HTML file from storage
            if (!string.IsNullOrEmpty(template.HtmlUrl))
            {
                await _fileStorageService.DeleteUdTemplateAsync(template.HtmlUrl);
            }

            // Delete the database record
            await _repository.DeleteTemplate(request.Id, request.UserId);
            return Unit.Value;
        }
    }
}