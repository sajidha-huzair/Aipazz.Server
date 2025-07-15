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
            Console.WriteLine($"Querying document with ID: {documentId}, UserID: {userId}");

            var sql = new QueryDefinition("SELECT * FROM c WHERE c.id = @id AND c.Userid = @userId")
                .WithParameter("@id", documentId)
            .WithParameter("@userId", userId);


            var iterator = _container.GetItemQueryIterator<Document>(sql);
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                var document = response.FirstOrDefault();
                if (document != null)
                    return document;
            }
            return null;

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

        public async Task<List<Document>> GetDocumentsByTeamIdsAsync(List<string> teamIds)
        {
            if (!teamIds.Any()) return new List<Document>();

            var query = new QueryDefinition(@"
                SELECT * FROM c 
                WHERE c.TeamId != null 
                AND ARRAY_CONTAINS(@teamIds, c.TeamId)")
                .WithParameter("@teamIds", teamIds);

            var iterator = _container.GetItemQueryIterator<Document>(query);
            var documents = new List<Document>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    documents.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching team shared documents: {ex.Message}");
                }
            }

            return documents;
        }

        public async Task<List<Document>> GetDocumentsByTeamIdAsync(string teamId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.TeamId = @teamId")
                .WithParameter("@teamId", teamId);

            var iterator = _container.GetItemQueryIterator<Document>(query);
            var documents = new List<Document>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    documents.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching documents for team {teamId}: {ex.Message}");
                }
            }

            return documents;
        }

        public async Task DeleteAsync(string documentId, string userId)
        {
            await _container.DeleteItemAsync<Document>(documentId, new PartitionKey(userId));
        }
    }
}
