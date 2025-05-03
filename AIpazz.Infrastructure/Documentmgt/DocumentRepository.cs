using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using Microsoft.Azure.Cosmos;

namespace AIpazz.Infrastructure.Documentmgt
{
    public class DocumentRepository : IdocumentRepository
    {

        private readonly Container _container;
        public DocumentRepository(CosmosClient clinet)
        {
            _container = clinet.GetContainer("Aipazz", "Document");
        }
        public Task<List<Document>> GetAllDocuments()
        {
            var documents = new List<Document>
            {
                new Document
                {
                    id = Guid.NewGuid().ToString(),
                    FileName = "Sample DDDD",
                    Url = "/doc.pdf"
                },
                new Document
                {
                    id = Guid.NewGuid().ToString(),
                    FileName = "Sample Document 2",
                    Url = "/doc.pdf"
                }
            };

            return Task.FromResult(documents);
        }

        public async Task SaveAsync(Document document)
        {
            await _container.CreateItemAsync(document, new PartitionKey(document.Userid));
        }
    }
}
