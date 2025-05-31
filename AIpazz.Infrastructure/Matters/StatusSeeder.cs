using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Aipazz.Domian;

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

        public async Task SeedDefaultStatusesAsync()
        {
            foreach (var statusName in DefaultStatuses)
            {
                var id = statusName.ToLower().Replace(" ", "-"); // e.g., "to-do"
                try
                {
                    await _container.ReadItemAsync<Status>(id, new PartitionKey(id));
                    // Already exists, skip.
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var status = new Status
                    {
                        id = id,
                        Name = id // id is also used as partition key
                    };

                    await _container.CreateItemAsync(status, new PartitionKey(status.Name));
                }
            }
        }
    }
}
