using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public UpdateDocumentHandler(IdocumentRepository repo)
        {
            _repo = repo;

        }
        public async Task<bool> Handle(UpdateDocumentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var doc = await _repo.GetByIdAsync(request.DocumentId, request.UserId);
            if (doc == null) return false;

            if (!string.IsNullOrWhiteSpace(request.FileName))
                doc.FileName = request.FileName;

            doc.LastModifiedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(doc.Url))
            {
                using (var ms = new MemoryStream())
                {
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                    {
                        MainDocumentPart mainpart = wordDoc.AddMainDocumentPart();
                        mainpart.Document = new Document(new Body());
                        HtmlConverter converter = new HtmlConverter(mainpart);
                        var paragraphs = converter.Parse(request.ContentHtml);
                        var body = mainpart.Document.Body;
                        foreach (var para in paragraphs)
                        {
                            body.Append(para);
                        }
                        mainpart.Document.Save();
                    }

                    // Save to file
                    await File.WriteAllBytesAsync(doc.Url, ms.ToArray(), cancellationToken);
                }

                await _repo.UpdateAsync(doc);
                return true;
            }

            // Fallback if doc.Url is empty or null
            return false;


        }
        }
    }

