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

        // Implement GetAllTimeEntries
        public async Task<List<ExpenseEntry>> GetAllExpenseEntries()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<ExpenseEntry>(query);
            List<ExpenseEntry> expenseEntries = new List<ExpenseEntry>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    expenseEntries.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching expense entries: {ex.Message}");
                }
            }

            return expenseEntries;
        }

        // Implement GetTimeEntryById
        public async Task<ExpenseEntry> GetExpenseEntryById(string id, string matterId)
        {
            try
            {
                var response = await _container.ReadItemAsync<ExpenseEntry>(
                id,
                    new PartitionKey(matterId) // Ensure partition key matches Cosmos DB setup
                );
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error fetching expense entry: {ex.Message}");
                return null;
            }
        }

        // Implement AddTimeEntry
        public async Task AddExpenseEntry(ExpenseEntry expenseEntry)
        {
            try
            {
                await _container.CreateItemAsync(expenseEntry, new PartitionKey(expenseEntry.matterId));
                Console.WriteLine($"Successfully added expense entry ID: {expenseEntry.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding expense entry: {ex.Message}");
            }
        }


        // Implement UpdateTimeEntry
        public async Task UpdateExpenseEntry(ExpenseEntry expenseEntry)
        {
            try
            {
                await _container.UpsertItemAsync(expenseEntry, new PartitionKey(expenseEntry.matterId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating expense entry: {ex.Message}");
            }
        }

        // Implement DeleteTimeEntry
        public async Task DeleteExpenseEntry(string id, string matterId)
        {
            try
            {
                await _container.DeleteItemAsync<ExpenseEntry>(id, new PartitionKey(matterId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting expense entry: {ex.Message}");
            }
        }
    }
}
