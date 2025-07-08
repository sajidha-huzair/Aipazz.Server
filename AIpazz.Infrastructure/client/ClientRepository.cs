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
using System.Net;

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
                    Console.WriteLine($"Error fetching clients: {ex.Message}");
                }
            }

            return clients;
        }

        public async Task<Client> GetByIdAsync(string id, string nic)
        {
            try
            {
                var response = await _container.ReadItemAsync<Client>(id, new PartitionKey(nic));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Client?> GetByNameAsync(string firstName, string lastName)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.FirstName = @firstName OR c.LastName = @lastName")
                .WithParameter("@firstName", firstName)
                .WithParameter("@lastName", lastName);
            var iterator = _container.GetItemQueryIterator<Client>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                var client = response.FirstOrDefault();
                if (client != null)
                    return client;
            }

            return null;
        }

        public async Task<Client?> GetByNicAsync(string nic)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.nic = @nic")
                .WithParameter("@nic", nic);

            using var iterator = _container.GetItemQueryIterator<Client>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(nic)
                });

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                var client = response.FirstOrDefault();
                if (client != null)
                    return client;
            }

            return null;
        }

        public async Task CreateAsync(Client client)
        {
            await _container.CreateItemAsync(client, new PartitionKey(client.nic));
        }

        public async Task UpdateAsync(Client client)
        {
            await _container.ReplaceItemAsync(client, client.id, new PartitionKey(client.nic));
        }

        public async Task DeleteAsync(string id, string nic)
        {
            await _container.DeleteItemAsync<Client>(id, new PartitionKey(nic));
        }
    }
}