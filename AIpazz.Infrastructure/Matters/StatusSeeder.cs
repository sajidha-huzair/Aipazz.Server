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
            "Open",
            "In Progress",
            "On Hold",
            "Closed"
        };

        public StatusSeeder(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["status"];
            _container = db.GetContainer(containerName);
        }

        public async Task SeedDefaultStatusesAsync(string userId)
        {
            // 1. Fetch existing statuses for the user
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

            // 2. Extract existing status names
            var existingNames = existingStatuses
                .Select(s => s.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 3. Add only missing statuses
            foreach (var statusName in DefaultStatuses)
            {
                if (!existingNames.Contains(statusName))
                {
                    var status = new Status
                    {
                        id = Guid.NewGuid().ToString(),
                        Name = statusName,
                        UserId = userId
                    };

                    await _container.CreateItemAsync(status, new PartitionKey(userId));
                }
            }
        }




    }
}
