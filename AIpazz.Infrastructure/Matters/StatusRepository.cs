using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

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
            List<Status> statuses = new();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                statuses.AddRange(response);
            }

            return statuses;
        }

        public async Task<Status?> GetStatusById(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Status>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddStatus(Status status)
        {
            status.id ??= Guid.NewGuid().ToString();
            await _container.CreateItemAsync(status, new PartitionKey(status.id));
        }

        public async Task UpdateStatus(Status status)
        {
            await _container.UpsertItemAsync(status, new PartitionKey(status.id));
        }

        public async Task DeleteStatus(string id)
        {
            var status = await GetStatusById(id);
            if (status is not null)
            {
                await _container.DeleteItemAsync<Status>(id, new PartitionKey(status.id));
            }
        }

    }
}
