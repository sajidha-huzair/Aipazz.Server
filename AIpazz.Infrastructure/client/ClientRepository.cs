using Aipazz.Application.client.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.client;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< Updated upstream
using System.Threading.Tasks;
using Aipazz.Application.client.Interfaces;
using Aipazz.Domian.client;
using Aipazz.Domian;
using Microsoft.Extensions.Options;
using Aipazz.Domian.Billing;
=======
using System.Net;
using System.Threading.Tasks;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
                try
                {
                    var response = await iterator.ReadNextAsync();
                    clients.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching expense entries: {ex.Message}");
                }
=======
                var response = await iterator.ReadNextAsync();
                clients.AddRange(response);
>>>>>>> Stashed changes
            }

            return clients;
        }

<<<<<<< Updated upstream
        public async Task<Client?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Client>(id, new PartitionKey(id));
                return response.Resource;
=======
        public async Task<Client?> GetByIdAsync(string id, string nic, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Client>(
                    id,
                    new PartitionKey(nic));

                return response.Resource?.UserId == userId ? response.Resource : null;
>>>>>>> Stashed changes
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

<<<<<<< Updated upstream
        public async Task<Client?> GetByNameAsync(string name)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
                .WithParameter("@name", name);
=======
        public async Task<Client?> GetByNameAsync(string firstName, string lastName, string userId)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE (c.FirstName = @firstName OR c.LastName = @lastName) AND c.userId = @userId")
                .WithParameter("@firstName", firstName)
                .WithParameter("@lastName", lastName)
                .WithParameter("@userId", userId);

>>>>>>> Stashed changes
            var iterator = _container.GetItemQueryIterator<Client>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

<<<<<<< Updated upstream
        public async Task<Client?> GetByNICAsync(string nic)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.nic = @nic")
                .WithParameter("@nic", nic);
            var iterator = _container.GetItemQueryIterator<Client>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
=======
        public async Task<Client?> GetByNicAsync(string nic, string userId)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.nic = @nic AND c.userId = @userId")
                .WithParameter("@nic", nic)
                .WithParameter("@userId", userId);

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
>>>>>>> Stashed changes
        }

        public async Task CreateAsync(Client client)
        {
            await _container.CreateItemAsync(client, new PartitionKey(client.nic));
        }

        public async Task UpdateAsync(Client client)
        {
            await _container.ReplaceItemAsync(client, client.id, new PartitionKey(client.nic));
        }

<<<<<<< Updated upstream
        public async Task DeleteAsync(string id)
        {
            var client = await GetByIdAsync(id);
            if (client != null)
            {
                await _container.DeleteItemAsync<Client>(id, new PartitionKey(client.nic));
=======
        public async Task DeleteAsync(string id, string nic, string userId)
        {
            // Optional: Check if the client belongs to the userId before deleting
            var client = await GetByIdAsync(id, nic, userId);
            if (client != null)
            {
                await _container.DeleteItemAsync<Client>(id, new PartitionKey(nic));
>>>>>>> Stashed changes
            }
        }
    }
}