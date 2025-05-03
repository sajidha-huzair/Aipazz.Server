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

        public DeleteDocumentHandler(IdocumentRepository repo,IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
            
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _repo.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                return false;
            var filepath = Path.Combine("UserDocuments", request.UserId, $"{document.FileName}.docx");
            if (File.Exists(filepath))
                File.Delete(filepath);

            await _repo.DeleteAsync(request.DocumentId , request.UserId);
            return true;
        }
    }
}
