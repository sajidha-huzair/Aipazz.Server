using Aipazz.Application.client.Interfaces;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Matters;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Matters.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public TaskRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["Tasks"];
            _container = db.GetContainer(containerName);
        }

        public async Task CreateAsync(MatterTask task)
        {
            await _container.CreateItemAsync(task, new PartitionKey(task.MatterId));
        }

        public async Task<MatterTask?> GetByIdAsync(string id, string matterId)
        {
            try
            {
                var response = await _container.ReadItemAsync<MatterTask>(id, new PartitionKey(matterId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<MatterTask>> GetAllAsync()
        {
            var query = _container.GetItemLinqQueryable<MatterTask>(true);
            var iterator = query.ToFeedIterator();
            var results = new List<MatterTask>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results; // Do NOT change this to List<MatterTask> if interface says IEnumerable<MatterTask>
        }

        public async Task<IEnumerable<MatterTask>> GetByMatterIdAsync(string matterId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.MatterId = @matterId")
                .WithParameter("@matterId", matterId);

            var iterator = _container.GetItemQueryIterator<MatterTask>(query, requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(matterId)
            });

            var results = new List<MatterTask>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<MatterTask?> GetByTitleAsync(string title)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Title = @title")
                .WithParameter("@title", title);

            var iterator = _container.GetItemQueryIterator<MatterTask>(query);
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task UpdateAsync(MatterTask task)
        {
            await _container.ReplaceItemAsync(task, task.id, new PartitionKey(task.MatterId));
        }

        public async Task DeleteAsync(string id, string matterId)
        {
            await _container.DeleteItemAsync<MatterTask>(id, new PartitionKey(matterId));
        }
    }
}