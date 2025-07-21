using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Domian.OtherDocuments;
using Microsoft.Azure.Cosmos;

namespace AIpazz.Infrastructure.OtherDocuments.Repository
{
    public class OtherDocumentRepository : IOtherDocumentRepository
    {
        private readonly Container _container;

        public OtherDocumentRepository(CosmosClient cosmosClient)
        {
            var database = cosmosClient.GetDatabase("Aipazz"); // Use your database name
            _container = database.GetContainer("OtherDocuments");
        }

        public async Task<string> SaveAsync(OtherDocument document)
        {
            await _container.CreateItemAsync(document, new PartitionKey(document.UserId));
            return document.id;  // Changed from Id to id
        }

        public async Task<OtherDocument?> GetByIdAsync(string id, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<OtherDocument>(id, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<OtherDocument>> GetAllByUserIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId OR c.TeamId != null")
                .WithParameter("@userId", userId);

            var results = new List<OtherDocument>();
            var iterator = _container.GetItemQueryIterator<OtherDocument>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<OtherDocument>> GetByMatterIdAsync(string matterId, string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.MatterId = @matterId AND (c.UserId = @userId OR c.TeamId != null)")
                .WithParameter("@matterId", matterId)
                .WithParameter("@userId", userId);

            var results = new List<OtherDocument>();
            var iterator = _container.GetItemQueryIterator<OtherDocument>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            try
            {
                await _container.DeleteItemAsync<OtherDocument>(id, new PartitionKey(userId));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(OtherDocument document)
        {
            try
            {
                document.LastModifiedAt = DateTime.UtcNow;
                await _container.ReplaceItemAsync(document, document.id, new PartitionKey(document.UserId));  // Changed from Id to id
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task ShareToTeamAsync(string documentId, string teamId, string userId)
        {
            var document = await GetByIdAsync(documentId, userId);
            if (document != null)
            {
                document.TeamId = teamId;
                await UpdateAsync(document);
            }
        }

        public async Task<bool> RemoveFromTeamAsync(string documentId, string userId)
        {
            var document = await GetByIdAsync(documentId, userId);
            if (document != null)
            {
                document.TeamId = null;
                await UpdateAsync(document);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<OtherDocument>> GetTeamSharedDocumentsAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.TeamId != null AND c.UserId = @userId")
                .WithParameter("@userId", userId);

            var results = new List<OtherDocument>();
            var iterator = _container.GetItemQueryIterator<OtherDocument>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}