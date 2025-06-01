using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Matters
{
    public class StatusSeeder
    {
        private readonly Container _container;

        private static readonly List<string> DefaultStatuses = new()
        {
            "To Do",
            "In Progress",
            "On Hold"
        };

        public StatusSeeder(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["status"];
            _container = db.GetContainer(containerName);
        }

        public async Task SeedDefaultStatusesAsync(string userId)
        {
            // 1. Check if the user already has statuses
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<Status>(query, requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(userId)
            });

            var existingStatuses = new List<Status>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                existingStatuses.AddRange(response);
            }

            // If statuses already exist for the user, skip seeding
            if (existingStatuses.Any())
                return;

            // 2. Seed default statuses for this user
            foreach (var statusName in DefaultStatuses)
            {
                var status = new Status
                {
                    id = Guid.NewGuid().ToString(),
                    Name = statusName,
                    UserId = userId // Ensure the userId is set
                };

                await _container.CreateItemAsync(status, new PartitionKey(userId));  // Use UserId as partition key
            }
        }



    }
}
