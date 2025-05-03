using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

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

        public async Task<Document?> GetByIdAsync(string documentId, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Document>(documentId, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task SaveAsync(Document document)
        {
            await _container.CreateItemAsync(document, new PartitionKey(document.Userid));
        }

        public async Task UpdateAsync(Document document)
        {
            await _container.ReplaceItemAsync(document, document.id, new PartitionKey(document.Userid));
        }

        public async Task<List<Document>> GetAllByUserIdAsync(string userId)
        {
            var query = _container.GetItemLinqQueryable<Document>(true)
                                  .Where(d => d.Userid == userId)
                                  .ToFeedIterator();

            var results = new List<Document>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

      

        public async Task DeleteAsync(string documentId, string userId)
        {
            await _container.DeleteItemAsync<Document>(documentId, new PartitionKey(userId));
        }
    }
}
