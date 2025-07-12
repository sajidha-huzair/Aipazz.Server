using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Handler
{
    public class CreateUdtemplatehandler : IRequestHandler<CreateUdtemplateCommand, string>
    {
        private readonly IUdtemplateRepository _repo;
        private readonly IFileStorageService _fileStorageService;

        public CreateUdtemplatehandler(IUdtemplateRepository repo, IFileStorageService filestorageservice)
        {
            _repo = repo;
            _fileStorageService = filestorageservice;
        }

        public async Task<string> Handle(CreateUdtemplateCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();
            var htmlUrl = await _fileStorageService.SaveUdTemplate(request.Udtemplate.Userid, id, request.Udtemplate.FileName, request.Udtemplate.ContentHtml);
            
            // Set the generated ID and URL
            request.Udtemplate.id = id;
            request.Udtemplate.HtmlUrl = htmlUrl;
            request.Udtemplate.CreatedAt = DateTime.UtcNow;
            request.Udtemplate.LastModifiedAt = DateTime.UtcNow;
            
            await _repo.CreateTemplate(request.Udtemplate);
            return request.Udtemplate.id;
        }
    }
}
