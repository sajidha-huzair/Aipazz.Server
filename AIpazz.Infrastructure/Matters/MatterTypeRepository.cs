using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Matters
{
    public class MatterTypeRepository : IMatterTypeRepository
    {
        private readonly Container _container;

        public MatterTypeRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["matterType"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<MatterType>> GetAllMatterTypes(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<MatterType>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(userId)
                });

            var result = new List<MatterType>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<MatterType?> GetMatterTypeById(string id, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<MatterType>(id, new PartitionKey(userId));
                var matterType = response.Resource;

                if (matterType.UserId != userId)
                    return null;

                return matterType;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<MatterType?> GetMatterTypeByName(string name, string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId AND c.Name = @name")
                .WithParameter("@userId", userId)
                .WithParameter("@name", name);

            var iterator = _container.GetItemQueryIterator<MatterType>(
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

        public async Task AddMatterType(MatterType type)
        {
            type.id ??= Guid.NewGuid().ToString();
            await _container.CreateItemAsync(type, new PartitionKey(type.UserId));
        }

        public async Task UpdateMatterType(MatterType type)
        {
            await _container.UpsertItemAsync(type, new PartitionKey(type.UserId));
        }

        public async Task DeleteMatterType(string id, string userId)
        {
            var existing = await GetMatterTypeById(id, userId);
            if (existing != null)
            {
                await _container.DeleteItemAsync<MatterType>(id, new PartitionKey(userId));
            }
        }


    }
}
