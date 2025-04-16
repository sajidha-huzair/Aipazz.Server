using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace AIpazz.Infrastructure.Documentmgt.Services
{
    public class WordGenerator : IWordGenerator
    {
        public byte[] GenerateFromHtml(string htmlContent)
        {
            using var mem = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                var altChunkId = "AltChunkId1";
                var chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);
                using var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));
                chunk.FeedData(htmlStream);

                var altChunk = new AltChunk { Id = altChunkId };
                mainPart.Document.Body.Append(altChunk);
                mainPart.Document.Save();
            }
            return mem.ToArray();
        }
    }
}

