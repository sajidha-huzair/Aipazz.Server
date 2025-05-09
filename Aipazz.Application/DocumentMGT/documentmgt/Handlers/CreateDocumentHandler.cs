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
        public CreateDocumentHandler(IdocumentRepository repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var userFolder = Path.Combine("UserDocuments", request.UserId);
            Directory.CreateDirectory(userFolder);
            var fileName = $"{Guid.NewGuid()}_{request.FileName}.docx";
            var fullpah = Path.Combine(userFolder, fileName);
            var htmlFilePath = Path.Combine(userFolder, fileName + ".html");

            using (var ms = new MemoryStream())
            {
                using(WordprocessingDocument wordDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document(new Body());
                    HtmlConverter converter = new HtmlConverter(mainPart);
                    var paragraphs = converter.Parse(request.ContentHtml);

                    Body body = mainPart.Document.Body;
                    foreach (var para in paragraphs)
                        body.Append(para);

                    mainPart.Document.Save();
                    
                }
                await File.WriteAllBytesAsync(fullpah, ms.ToArray(), cancellationToken);
            }
            await File.WriteAllTextAsync(htmlFilePath, request.ContentHtml, cancellationToken);

            //save metadata
            var document = new Aipazz.Domian.DocumentMgt.Document
            {
                FileName = request.FileName,
                Userid = request.UserId,
                Url = fullpah.Replace("\\", "/"),
                HtmlUrl = htmlFilePath.Replace("\\", "/"),
            };

            await _repo.SaveAsync(document);
            return document.id;
           
            
        }
    }
}
