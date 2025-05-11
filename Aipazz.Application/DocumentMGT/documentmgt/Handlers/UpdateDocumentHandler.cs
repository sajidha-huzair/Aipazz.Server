using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class UpdateDocumentHandler : IRequestHandler<UpdateDocumentCommand, bool>
    {
        private readonly IdocumentRepository _repo;
        private readonly IFileStorageService _fileStorageService;

        public UpdateDocumentHandler(IdocumentRepository repo, IFileStorageService fileStorageService)
        {
            _repo = repo;
            _fileStorageService = fileStorageService;
        }

        public async Task<bool> Handle(UpdateDocumentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var doc = await _repo.GetByIdAsync(request.DocumentId, request.UserId);
            if (doc == null) return false;

            if (!string.IsNullOrWhiteSpace(request.FileName))
                doc.FileName = request.FileName;

            doc.LastModifiedAt = DateTime.UtcNow;


           

            // Generate Word document in memory
            byte[] wordBytes;
            using (var ms = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    var mainpart = wordDoc.AddMainDocumentPart();
                    mainpart.Document = new Document(new Body());
                    var converter = new HtmlConverter(mainpart);
                    var paragraphs = converter.Parse(request.ContentHtml);
                    foreach (var para in paragraphs)
                    {
                        mainpart.Document.Body.Append(para);
                    }
                    mainpart.Document.Save();
                }
                wordBytes = ms.ToArray();
            }

            // Save Word document
            doc.Url = await _fileStorageService.UpdateWordDocumentAsync(request.UserId, request.DocumentId, request.FileName, wordBytes);

            // Save HTML content
            doc.HtmlUrl = await _fileStorageService.UpdateHtmlContentAsync(request.UserId, request.DocumentId, request.FileName, request.ContentHtml);

            await _repo.UpdateAsync(doc);
            return true;
        }
    }
}
