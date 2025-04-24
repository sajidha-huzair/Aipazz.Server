using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using HtmlToOpenXml;

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
                mainPart.Document = new Document();
                var body = new Body();

                // HtmlToOpenXml magic happens here 👇
                var converter = new HtmlConverter(mainPart);
                var paragraphs = converter.Parse(htmlContent); // returns OpenXML elements

                foreach (var para in paragraphs)
                {
                    body.Append(para);
                }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            return mem.ToArray();
        }
    }
}

