using Aipazz.Application.client.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.client;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aipazz.Infrastructure.client
{
    public class ClientRepository : IClientRepository
    {
        private readonly Container _container;

        public ClientRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["client"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<Client>> GetAllClients(string userId)
        {
            var iterator = _container.GetItemLinqQueryable<Client>()
                .Where(c => c.UserId == userId)
                .ToFeedIterator();

            var clients = new List<Client>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                clients.AddRange(response);
            }

            return clients;
        }

        public async Task<Client?> GetByIdAsync(string id, string nic, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Client>(
                    id,
                    new PartitionKey(nic));

                return response.Resource?.UserId == userId ? response.Resource : null;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Client?> GetByNameAsync(string firstName, string lastName, string userId)
        {
            var query = new QueryDefinition(
    "SELECT * FROM c WHERE (c.FirstName = @firstName OR c.LastName = @lastName) AND c.UserId = @userId")
    .WithParameter("@firstName", firstName)
    .WithParameter("@lastName", lastName)
    .WithParameter("@userId", userId);


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

        public async Task<Client?> GetByNicAsync(string nic, string userId)
        {
            var iterator = _container.GetItemLinqQueryable<Client>()
                .Where(c => c.nic == nic && c.UserId == userId)
                .Take(1)
                .ToFeedIterator();

            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
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

        public async Task DeleteAsync(string id, string nic, string userId)
        {
            // Optional: Check if the client belongs to the userId before deleting
            var client = await GetByIdAsync(id, nic, userId);
            if (client != null)
            {
                await _container.DeleteItemAsync<Client>(id, new PartitionKey(nic));
            }
        }
    }
}