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
    public class ExpenseEntryRepository : IExpenseEntryRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;


        public ExpenseEntryRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["ExpenseEntry"];
            _container = db.GetContainer(containerName);
        }

        // Implement GetAllExpenseEntries
        public async Task<List<ExpenseEntry>> GetAllExpenseEntries(string userId)
        {
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(allowSynchronousQueryExecution: false)
                                  .Where(t => t.UserId == userId)
                                  .AsQueryable();

            var iterator = query.ToFeedIterator();
            var results = new List<ExpenseEntry>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        // Implement GetExpenseEntryById
        public async Task<ExpenseEntry> GetExpenseEntryById(string id, string matterId, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<ExpenseEntry>(id, new PartitionKey(matterId));
                return response.Resource?.UserId == userId ? response.Resource : null;
            }
            catch
            {
                return null;
            }
        }
        
        
        // Implement AddExpenseEntry
        public async Task AddExpenseEntry(ExpenseEntry expenseEntry)
        {
            try
            {
                await _container.CreateItemAsync(expenseEntry, new PartitionKey(expenseEntry.matterId));
                Console.WriteLine($"Successfully added entry ID: {expenseEntry.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding entry: {ex.Message}");
            }
        }


        // Implement UpdateExpenseEntry
        public async Task UpdateExpenseEntry(ExpenseEntry expenseEntry)
        {
            var existing = await GetExpenseEntryById(expenseEntry.id, expenseEntry.matterId, expenseEntry.UserId);
            if (existing == null) return;

            await _container.UpsertItemAsync(expenseEntry, new PartitionKey(expenseEntry.matterId));

        }

        // Implement DeleteExpenseEntry
        public async Task DeleteExpenseEntry(string id, string matterId, string userId)
        {
            var entry = await GetExpenseEntryById(id, matterId, userId);
            if (entry != null)
            {
                await _container.DeleteItemAsync<ExpenseEntry>(id, new PartitionKey(matterId));
            }
        }

        public async Task<List<ExpenseEntry>> GetExpenseEntriesByMatterIdAsync(string matterId, string userId)
        {
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(allowSynchronousQueryExecution: false)
                                 .Where(t => t.matterId == matterId && t.UserId == userId)
                                 .ToFeedIterator();

            var expenseEntries = new List<ExpenseEntry>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                expenseEntries.AddRange(response);
            }

            return expenseEntries;
        }
        public async Task<List<ExpenseEntry>> GetAllEntriesByIdsAsync(List<string> entryIds, string userId)
        {
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(true)
                .Where(e => entryIds.Contains(e.id) && e.UserId == userId)
                .ToFeedIterator();

            var results = new List<ExpenseEntry>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        //Fetch only unbilled entries(for UI list)
        public async Task<List<ExpenseEntry>> GetUnbilledByMatterIdAsync(string matterId,
                                                                 string userId)
        {
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(
                 allowSynchronousQueryExecution: true,      // or false
                 continuationToken: null,
                 requestOptions: new QueryRequestOptions
                 {
                     PartitionKey = new PartitionKey(userId)
                 })
              .Where(e => e.matterId == matterId &&
                          e.UserId == userId &&
                          e.InvoiceId == null)
              .ToFeedIterator();


            var results = new List<ExpenseEntry>();
            while (query.HasMoreResults)
            {
                var page = await query.ReadNextAsync();
                results.AddRange(page);
            }
            return results;
        }

        public async Task MarkEntriesInvoicedAsync(IEnumerable<string> entryIds,
                                           string invoiceId,
                                           string userId)
        {
            //  Pull only the entries we need (so we know their matterId)
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(true)
                                  .Where(e => entryIds.Contains(e.id) && e.UserId == userId)
                                  .ToFeedIterator();

            var entries = new List<ExpenseEntry>();
            while (query.HasMoreResults)
                entries.AddRange(await query.ReadNextAsync());

            // Update & replace using the partition key (matterId)
            foreach (var entry in entries)
            {
                entry.InvoiceId = invoiceId;
                await _container.ReplaceItemAsync(entry,
                                                  entry.id,
                                                  new PartitionKey(entry.matterId));
            }
        }

        public async Task<List<ExpenseEntry>> GetBilledByMatterIdAsync(string matterId,
                                                            string userId)
        {
            var query = _container.GetItemLinqQueryable<ExpenseEntry>(
                            allowSynchronousQueryExecution: true,
                            continuationToken: null,
                            requestOptions: new QueryRequestOptions
                            {
                                PartitionKey = new PartitionKey(userId)
                            })
                        .Where(e => e.matterId == matterId &&
                                    e.UserId == userId &&
                                    e.InvoiceId != null)     // billed
                        .ToFeedIterator();

            var list = new List<ExpenseEntry>();
            while (query.HasMoreResults)
                list.AddRange(await query.ReadNextAsync());
            return list;
        }


    }
}
