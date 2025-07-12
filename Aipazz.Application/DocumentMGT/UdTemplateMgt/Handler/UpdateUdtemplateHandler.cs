using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Handler
{
    public class UpdateUdtemplateHandler : IRequestHandler<UpdateUdtemplateCommand, Unit>
    {
        private readonly IUdtemplateRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public UpdateUdtemplateHandler(IUdtemplateRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(UpdateUdtemplateCommand request, CancellationToken cancellationToken)
        {
            // Get the existing template to retrieve the old URL
            var existingTemplate = await _repository.GetTemplateByIdAndUserId(request.Udtemplate.id, request.Udtemplate.Userid);
            if (existingTemplate == null)
            {
                throw new KeyNotFoundException($"Template with ID {request.Udtemplate.id} not found for user {request.Udtemplate.Userid}.");
            }

            // Delete the old file from storage if it exists
            if (!string.IsNullOrEmpty(existingTemplate.HtmlUrl))
            {
                await _fileStorageService.DeleteUdTemplateAsync(existingTemplate.HtmlUrl);
            }

            // Save the new template file with updated content
            var newHtmlUrl = await _fileStorageService.SaveUdTemplate(request.Udtemplate.Userid, request.Udtemplate.id, request.Udtemplate.FileName, request.Udtemplate.ContentHtml);
            
            // Update the template with new URL and timestamp
            request.Udtemplate.HtmlUrl = newHtmlUrl;
            request.Udtemplate.LastModifiedAt = DateTime.UtcNow;
            request.Udtemplate.CreatedAt = existingTemplate.CreatedAt; // Preserve original creation time
            
            await _repository.UpdateTemplate(request.Udtemplate);
            return Unit.Value;
        }
    }
}