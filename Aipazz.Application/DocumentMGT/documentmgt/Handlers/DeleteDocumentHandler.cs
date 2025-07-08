using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class DeleteDocumentHandler : IRequestHandler<DeleteDocumentCommand,bool>
    {
        private readonly IdocumentRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IFileStorageService _fileStorageService;

        public DeleteDocumentHandler(IdocumentRepository repo,IWebHostEnvironment env, IFileStorageService fileStorageService)
        {
            _repo = repo;
            _env = env;
            _fileStorageService = fileStorageService;
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _repo.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                return false;

            var response = await _fileStorageService.DeleteDocumentAsync(document.Url, document.HtmlUrl);
            Console.WriteLine(response);

            await _repo.DeleteAsync(request.DocumentId, request.UserId);
            return true;

            
        }
    }
}
