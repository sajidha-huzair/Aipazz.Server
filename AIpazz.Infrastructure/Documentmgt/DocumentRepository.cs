using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;

namespace AIpazz.Infrastructure.Documentmgt
{
    public class DocumentRepository : IdocumentRepository
    {
        public Task<List<Document>> GetAllDocuments()
        {
            var documents = new List<Document>
            {
                new Document
                {
                    id = Guid.NewGuid().ToString(),
                    name = "Sample DDDD",
                    Url = "/doc.pdf"
                },
                new Document
                {
                    id = Guid.NewGuid().ToString(),
                    name = "Sample Document 2",
                    Url = "/doc.pdf"
                }
            };

            return Task.FromResult(documents);
        }
    }
}
