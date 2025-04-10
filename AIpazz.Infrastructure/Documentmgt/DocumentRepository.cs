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
                    name = "Sample Document 1",
                    Url = "https://example.com/doc1.pdf"
                },
                new Document
                {
                    id = Guid.NewGuid().ToString(),
                    name = "Sample Document 2",
                    Url = "https://example.com/doc2.pdf"
                }
            };

            return Task.FromResult(documents);
        }
    }
}
