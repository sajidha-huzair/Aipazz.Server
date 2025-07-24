using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian.Billing;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;

namespace AIpazz.Infrastructure.Billing
{
    /// <summary>
    /// Repository for managing Invoice entities using Azure Cosmos DB.
    /// </summary>
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly Container _container;

        public InvoiceRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["invoice"];
            _container = db.GetContainer(containerName);
        }

        // ───────────── GET ALL FOR USER  ─────────────
        public async Task<List<Invoice>> GetAllForUserAsync(string userId)
        {
            // Same implementation as GetAllInvoicesByUserId
            var query = _container.GetItemLinqQueryable<Invoice>(
                            requestOptions: new QueryRequestOptions
                            {
                                PartitionKey = new PartitionKey(userId)
                            },
                            allowSynchronousQueryExecution: false)
                        .Where(i => i.UserId == userId)
                        .ToFeedIterator();

            var results = new List<Invoice>();
            while (query.HasMoreResults)
            {
                var page = await query.ReadNextAsync();
                results.AddRange(page);
            }
            return results;
        }
        public async Task CreateAsync(Invoice invoice)
        {
            await _container.CreateItemAsync(invoice, new PartitionKey(invoice.UserId));
        }

        public async Task<Invoice?> GetLastInvoiceAsync(string userId)
        {
            var query = _container.GetItemLinqQueryable<Invoice>(true)
                                  .Where(i => i.UserId == userId)
                                  .OrderByDescending(i => i.CreatedAt)
                                  .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var page = await query.ReadNextAsync();
                var first = page.FirstOrDefault();
                if (first != null) return first;
            }

            return null;
        }

        // ────────────────────── GET SINGLE BY ID ──────────────────────
        public async Task<Invoice?> GetByIdAsync(string id, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Invoice>(
                                   id,
                                   new PartitionKey(userId));   // partition key = UserId
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; 
            }
        }

        // UPDATE
        public async Task UpdateAsync(Invoice invoice)
        {
            
            await _container.ReplaceItemAsync(
                item: invoice,
                id: invoice.id,
                partitionKey: new PartitionKey(invoice.UserId));
        }

        // DELETE
        public async Task DeleteAsync(string id, string userId)
        {
            try
            {
                await _container.DeleteItemAsync<Invoice>(
                    id: id,
                    partitionKey: new PartitionKey(userId));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                
            }
        }




    }

}
