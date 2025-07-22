using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Matters
{
    public class MatterTypeSeeder
    {
        private readonly Container _container;

        private static readonly List<string> DefaultMatterTypes = new()
        {
            "Civil",
            "Criminal",
            "Administrative",
            "Constitutional",
            "Commercial"
        };

        public MatterTypeSeeder(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["matterType"];
            _container = db.GetContainer(containerName);
        }

        public async Task SeedDefaultMatterTypesAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<MatterType>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(userId)
                });

            var existingTypes = new List<MatterType>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                existingTypes.AddRange(response);
            }

            var existingNames = existingTypes
                .Select(t => t.Name)
                .ToHashSet(System.StringComparer.OrdinalIgnoreCase);

            foreach (var typeName in DefaultMatterTypes)
            {
                if (!existingNames.Contains(typeName))
                {
                    var matterType = new MatterType
                    {
                        id = Guid.NewGuid().ToString(),
                        Name = typeName,
                        UserId = userId
                    };

                    await _container.CreateItemAsync(matterType, new PartitionKey(userId));
                }
            }
        }
    }
}
