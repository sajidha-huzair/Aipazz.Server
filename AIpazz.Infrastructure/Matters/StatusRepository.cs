using System;
using System.Collections.Generic;
using System.Linq;
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

        // ✅ Get all statuses for a user using /UserId as partition key
        public async Task<List<Status>> GetAllStatuses(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<Status>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(userId)
                });

            List<Status> statuses = new();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                statuses.AddRange(response);
            }

            return statuses;
        }

        // ✅ Read with /UserId as partition key
        public async Task<Status?> GetStatusById(string id, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Status>(id, new PartitionKey(userId));
                var status = response.Resource;

                if (status.UserId != userId)
                    return null;

                return status;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // ✅ Write with /UserId as partition key
        public async Task AddStatus(Status status)
        {
            status.id ??= Guid.NewGuid().ToString();
            await _container.CreateItemAsync(status, new PartitionKey(status.UserId));
        }

        // ✅ Upsert with /UserId as partition key
        public async Task UpdateStatus(Status status)
        {
            await _container.UpsertItemAsync(status, new PartitionKey(status.UserId));
        }

        // ✅ Delete with /UserId as partition key
        public async Task DeleteStatus(string id, string userId)
        {
            var status = await GetStatusById(id, userId);
            if (status is not null)
            {
                await _container.DeleteItemAsync<Status>(id, new PartitionKey(userId));
            }
        }

        public async Task<Status?> GetStatusByName(string name, string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId AND c.Name = @name")
                .WithParameter("@userId", userId)
                .WithParameter("@name", name);

            var iterator = _container.GetItemQueryIterator<Status>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(userId)
                });

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                var result = response.FirstOrDefault();
                if (result != null)
                    return result;
            }

            return null;
        }

    }
}
