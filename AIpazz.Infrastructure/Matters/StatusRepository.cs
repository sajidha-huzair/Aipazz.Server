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
    public class StatusRepository : IStatusRepository
    {
        private readonly Container _container;

        public StatusRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["status"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Status>(query);
            List<Status> statuses = new List<Status>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    statuses.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching statuses: {ex.Message}");
                }
            }

            return statuses;
        }

        public async Task<Status> GetStatusById(string id, string Name)
        {
            try
            {
                var response = await _container.ReadItemAsync<Status>(
                    id,
                    new PartitionKey(Name)
                );
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error fetching status by ID: {ex.Message}");
                return null;
            }
        }

        public async Task AddStatus(Status status)
        {
            try
            {
                await _container.CreateItemAsync(status, new PartitionKey(status.Name));
                Console.WriteLine($"Successfully added status ID: {status.Id}, Name: {status.Name}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding status: {ex.Message}");
            }
        }

        public async Task UpdateStatus(Status status)
        {
            try
            {
                await _container.UpsertItemAsync(status, new PartitionKey(status.Name));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating status: {ex.Message}");
            }
        }

        public async Task DeleteStatus(string id, string Name)
        {
            try
            {
                await _container.DeleteItemAsync<Status>(id, new PartitionKey(Name));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting status: {ex.Message}");
            }
        }

        public async Task<List<Status>> GetStatusesByNameAsync(string Name)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @name")
                .WithParameter("@name", Name);

            var iterator = _container.GetItemQueryIterator<Status>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(Name)
                });

            List<Status> statuses = new();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                statuses.AddRange(response);
            }

            return statuses;
        }
    }
}
