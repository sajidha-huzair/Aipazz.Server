using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aipazz.Infrastructure.Matters
{
    public class MatterRepository : IMatterRepository
    {
        private readonly Container _container;

        public MatterRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["matter"];
            _container = db.GetContainer(containerName);
        }

        // 1. GetAllMatters
        public async Task<List<Matter>> GetAllMatters(string userId)
        {
            var query = _container.GetItemLinqQueryable<Matter>()
                .Where(m => m.UserId == userId)
                .ToFeedIterator();

            List<Matter> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        // 2. GetMatterById
        public async Task<Matter?> GetMatterById(string id, string clientNic, string userId)
        {
            var query = _container
                .GetItemLinqQueryable<Matter>()
                .Where(m => m.id == id && m.ClientNic == clientNic && m.UserId == userId)
                .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return null;
        }

        // 3. AddMatter
        public async Task AddMatter(Matter matter)
        {
            try
            {
                Console.WriteLine("Saving Matter to Cosmos DB:");
                Console.WriteLine(JsonSerializer.Serialize(matter, new JsonSerializerOptions { WriteIndented = true }));

                await _container.CreateItemAsync(matter, new PartitionKey(matter.ClientNic));

                Console.WriteLine($"✅ Successfully added matter ID: {matter.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"❌ Cosmos DB Error adding matter: {ex.StatusCode} - {ex.Message}");
                Console.WriteLine($"ActivityId: {ex.ActivityId}");

                // Re-throw to let upper layers handle the error (e.g., controller or handler)
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ General error while adding matter: {ex.Message}");
                throw;
            }
        }

        // 4. UpdateMatter
        public async Task UpdateMatter(Matter matter)
        {
            try
            {
                await _container.UpsertItemAsync(matter, new PartitionKey(matter.ClientNic));
                Console.WriteLine($"✅ Successfully updated matter ID: {matter.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"❌ Cosmos DB Error updating matter: {ex.StatusCode} - {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ General error while updating matter: {ex.Message}");
                throw;
            }
        }

        // 5. DeleteMatter
        public async Task DeleteMatter(string id, string clientNic, string userId)
        {
            var matter = await GetMatterById(id, clientNic, userId);
            if (matter is not null)
            {
                await _container.DeleteItemAsync<Matter>(id, new PartitionKey(clientNic));
                Console.WriteLine($"✅ Successfully deleted matter ID: {id}");
            }
        }
        public async Task<List<Matter>> GetMattersByIdsAsync(List<string> matterIds, string userId)
        {
            var query = _container.GetItemLinqQueryable<Matter>(true)
                .Where(m => matterIds.Contains(m.id) && m.UserId == userId)
                .ToFeedIterator();

            var results = new List<Matter>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        // 6. GetMattersByClientNicAsync
        public async Task<List<Matter>> GetMattersByClientNicAsync(string clientNic, string userId)
        {
            var query = _container.GetItemLinqQueryable<Matter>()
                .Where(m => m.ClientNic == clientNic && m.UserId == userId)
                .ToFeedIterator();

            List<Matter> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
