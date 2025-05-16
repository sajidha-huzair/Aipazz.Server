using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Infrastructure.Matters
{
    public class MatterRepository : IMatterRepository
    {
        private readonly Container _container;

        public MatterRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["matters"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<Matter>> GetAllMatters()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Matter>(query);
            List<Matter> matters = new List<Matter>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    matters.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching matters: {ex.Message}");
                }
            }

            return matters;
        }

        public async Task<Matter> GetMatterById(string id, string title)
        {
            try
            {
                var response = await _container.ReadItemAsync<Matter>(
                    id,
                    new PartitionKey(title)
                );
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error fetching matter: {ex.Message}");
                return null;
            }
        }

        public async Task AddMatter(Matter matter)
        {
            try
            {
                // Ensure CourtType is populated at this point
                await _container.CreateItemAsync(matter, new PartitionKey(matter.title));
                Console.WriteLine($"Successfully added matter ID: {matter.id}, CourtType: {matter.CourtType}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding matter: {ex.Message}");
            }
        }

        public async Task UpdateMatter(Matter matter)
        {
            try
            {
                await _container.UpsertItemAsync(matter, new PartitionKey(matter.title));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating matter: {ex.Message}");
            }
        }

        public async Task DeleteMatter(string id, string title)
        {
            try
            {
                await _container.DeleteItemAsync<Matter>(id, new PartitionKey(title));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting matter: {ex.Message}");
            }
        }
    }
}
