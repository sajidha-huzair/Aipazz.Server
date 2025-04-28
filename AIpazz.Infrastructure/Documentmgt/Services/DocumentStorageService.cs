using System;
using System.IO;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml; // <-- Add this
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph; // To avoid confusion

namespace AIpazz.Infrastructure.Documentmgt.Services
{
    public class DocumentStorageService : IDocumentStorageService
    {
        private readonly string _documentStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents");

        public DocumentStorageService()
        {
            if (!Directory.Exists(_documentStoragePath))
            {
                Directory.CreateDirectory(_documentStoragePath);
            }
        }

        public async Task<string> SaveDocumentAsync(string fileName, string contentHtml)
        {
            string finalFileName = fileName.EndsWith(".docx") ? fileName : $"{fileName}.docx";
            string fullPath = Path.Combine(_documentStoragePath, finalFileName);

            using (MemoryStream mem = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(mem, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    // Add MainDocumentPart
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document(new Body());

                    // Now use HtmlConverter from HtmlToOpenXml
                    var converter = new HtmlConverter(mainPart);

                    // Optional: customize styles if you want (not required now)
                    // converter.DefaultParagraphStyle = "Normal";

                    // Convert the HTML string into OpenXML elements
                    var paragraphs = converter.Parse(contentHtml);

                    // Add the parsed paragraphs into the Body
                    mainPart.Document.Body.Append(paragraphs);

                    mainPart.Document.Save();
                }

                await File.WriteAllBytesAsync(fullPath, mem.ToArray());
            }

            return finalFileName;
        }
    }
}
