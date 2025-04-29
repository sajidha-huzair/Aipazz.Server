using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class SaveDocumentCommandHandler:IRequestHandler<SaveDocumentCommand,string>
    {
        private readonly IDocumentStorageService _documentStorageService;

        public SaveDocumentCommandHandler(IDocumentStorageService documentStorageService)
        { 
            _documentStorageService = documentStorageService;
  
        }

        public async Task<string> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
        {
            var savedFileName = await _documentStorageService.SaveDocumentAsync(request.FileName, request.ContentHtml);
            return savedFileName;
        }
    }
}
