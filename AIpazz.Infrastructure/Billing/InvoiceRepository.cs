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

        public async Task<List<Invoice>> GetAllInvoicesByUserId(string userId)
        {
            // Build a LINQ query to filter invoices by UserId
            // Use partition key to optimize performance and RU consumption
            var query = _container.GetItemLinqQueryable<Invoice>(
                            requestOptions: new QueryRequestOptions
                            {
                                PartitionKey = new PartitionKey(userId) 
                            },
                            allowSynchronousQueryExecution: false)
                        .Where(i => i.UserId == userId)
                        .ToFeedIterator();

            var invoices = new List<Invoice>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                invoices.AddRange(response);
            }

            return invoices;
        }

    }

}
