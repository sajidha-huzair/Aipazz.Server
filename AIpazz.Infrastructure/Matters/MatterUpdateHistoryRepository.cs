using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aipazz.Infrastructure.Matters
{
    public class MatterUpdateHistoryRepository : IMatterUpdateHistoryRepository
    {
        private readonly Container _container;

        public MatterUpdateHistoryRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["matterUpdateHistory"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<MatterUpdateHistory>> GetMatterUpdateHistory(string matterId, string clientNic, string userId)
        {
            var queryText = "SELECT * FROM c WHERE c.MatterId = @matterId AND c.ClientNic = @clientNic AND c.UserId = @userId";
            var queryDef = new QueryDefinition(queryText)
                .WithParameter("@matterId", matterId)
                .WithParameter("@clientNic", clientNic)
                .WithParameter("@userId", userId);

            var query = _container.GetItemQueryIterator<MatterUpdateHistory>(
                queryDef,
                requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
            );

            var results = new List<MatterUpdateHistory>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddMatterUpdateHistory(MatterUpdateHistory updateHistory)
        {
            try
            {
                Console.WriteLine("Saving MatterUpdateHistory to separate container:");
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(updateHistory, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                // Use UserId as partition key
                await _container.CreateItemAsync(updateHistory, new PartitionKey(updateHistory.UserId));

                Console.WriteLine($"✅ Successfully added update history ID: {updateHistory.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"❌ Cosmos DB Error adding update history: {ex.StatusCode} - {ex.Message}");
                Console.WriteLine($"ActivityId: {ex.ActivityId}");
                throw;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"❌ General error while adding update history: {ex.Message}");
                throw;
            }
        }
    }
}