using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand, Unit>
    {
        private readonly ITemplateRepository _repository;
        private readonly IFileStorageService _fileStorage;

        public DeleteTemplateHandler(ITemplateRepository repository, IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<Unit> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            // First, get the template to retrieve the file URL
            var template = await _repository.GetTemplateById(request.Id);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {request.Id} not found.");
            }

            // Delete the HTML file from storage
            if (!string.IsNullOrEmpty(template.Url))
            {
                await _fileStorage.DeleteTemplateAsync(template.Url);
            }

            // Delete the database record
            await _repository.DeleteTemplate(request.Id);
            return Unit.Value;
        }
    }
}
