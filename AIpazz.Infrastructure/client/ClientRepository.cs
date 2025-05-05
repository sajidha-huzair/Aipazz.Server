using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aipazz.Application.client.Interfaces;
using Aipazz.Domian.client;
using Aipazz.Domian;
using Microsoft.Extensions.Options;
using Aipazz.Domian.Billing;

namespace Aipazz.Infrastructure.client
{
    public class ClientRepository : IClientRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public ClientRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["client"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<Client>> GetAllClients()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Client>(query);
            List<Client> clients = new List<Client>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    clients.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching expense entries: {ex.Message}");
                }
            }

            return clients;
        }

        public async Task<Client?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Client>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Client?> GetByNameAsync(string name)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
                .WithParameter("@name", name);
            var iterator = _container.GetItemQueryIterator<Client>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<Client?> GetByNICAsync(string nic)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.nic = @nic")
                .WithParameter("@nic", nic);
            var iterator = _container.GetItemQueryIterator<Client>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task CreateAsync(Client client)
        {
            await _container.CreateItemAsync(client, new PartitionKey(client.nic));
        }

        public async Task UpdateAsync(Client client)
        {
            await _container.ReplaceItemAsync(client, client.id, new PartitionKey(client.nic));
        }

        public async Task DeleteAsync(string id)
        {
            var client = await GetByIdAsync(id);
            if (client != null)
            {
                await _container.DeleteItemAsync<Client>(id, new PartitionKey(client.nic));
            }
        }
    }
}