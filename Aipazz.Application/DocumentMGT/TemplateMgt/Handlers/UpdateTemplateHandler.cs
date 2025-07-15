using System;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class UpdateTemplateHandler : IRequestHandler<UpdateTemplateCommand, Unit>
    {
        private readonly ITemplateRepository _repository;
        private readonly IFileStorageService _fileStorage;

        public UpdateTemplateHandler(ITemplateRepository repository, IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<Unit> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            // Get the existing template to retrieve the old URL
            var existingTemplate = await _repository.GetTemplateById(request.Template.id);
            if (existingTemplate == null)
            {
                throw new KeyNotFoundException($"Template with ID {request.Template.id} not found.");
            }

            // Delete the old file from storage if it exists
            if (!string.IsNullOrEmpty(existingTemplate.Url))
            {
                await _fileStorage.DeleteTemplateAsync(existingTemplate.Url);
            }

            // Save the new template file with updated content/name
            var newUrl = await _fileStorage.SaveTemplateAsync(request.Template.id, request.Template.Name, request.Template.ContentHtml);
            
            // Update the template with new URL and timestamp
            request.Template.Url = newUrl;
            request.Template.LastModified = DateTime.UtcNow;
            request.Template.CreatedAt = existingTemplate.CreatedAt; // Preserve original creation time
            
            await _repository.UpdateTemplate(request.Template);
            return Unit.Value;
        }
    }
}
