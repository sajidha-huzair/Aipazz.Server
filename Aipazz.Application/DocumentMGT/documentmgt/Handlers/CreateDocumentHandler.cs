using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using MediatR;
using HtmlToOpenXml;



namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class CreateDocumentHandler : IRequestHandler<CreateDocumentCommand, string>
    {
        private readonly IdocumentRepository _repo;
        private readonly IFileStorageService _fileStorageService;

        public CreateDocumentHandler(IdocumentRepository repo, IFileStorageService fileStorageService)
        {
            _repo = repo;
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            byte[] wordContent;
            var documentId = Guid.NewGuid().ToString();


            using (var ms = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document(new Body());
                    var converter = new HtmlConverter(mainPart);
                    var paragraphs = converter.Parse(request.ContentHtml);

                    var body = mainPart.Document.Body;
                    foreach (var para in paragraphs)
                        body.Append(para);

                    mainPart.Document.Save();
                }

                wordContent = ms.ToArray();
            }

            var wordUrl = await _fileStorageService.SaveWordDocumentAsync(request.UserId, documentId, request.FileName, wordContent);
            var htmlUrl = await _fileStorageService.SaveHtmlContentAsync(request.UserId, documentId, request.FileName, request.ContentHtml);

            var document = new Aipazz.Domian.DocumentMgt.Document
            {
                FileName = request.FileName,
                Userid = request.UserId,
                Url = wordUrl,
                HtmlUrl = htmlUrl,
            };

            await _repo.SaveAsync(document);
            return document.id;
        }
    }
}
