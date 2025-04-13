using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domain.Billing;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AIpazz.Infrastructure.Billing
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;


        public TimeEntryRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["TimeEntry"];
            _container = db.GetContainer(containerName);
        }

        // Implement GetAllTimeEntries
        public async Task<List<TimeEntry>> GetAllTimeEntries()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<TimeEntry>(query);
            List<TimeEntry> timeEntries = new List<TimeEntry>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    timeEntries.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching time entries: {ex.Message}");
                }
            }

            return timeEntries;
        }

        // Implement GetTimeEntryById
        public async Task<TimeEntry> GetTimeEntryById(string id, int matterId)
        {
            try
            {
                var response = await _container.ReadItemAsync<TimeEntry>(
                id,
                    new PartitionKey(matterId) // Ensure partition key matches Cosmos DB setup
                );
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error fetching time entry: {ex.Message}");
                return null;
            }
        }

        // Implement AddTimeEntry
        public async Task AddTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                await _container.CreateItemAsync(timeEntry, new PartitionKey(timeEntry.matterId));
                Console.WriteLine($"Successfully added time entry ID: {timeEntry.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding time entry: {ex.Message}");
            }
        }


        // Implement UpdateTimeEntry
        public async Task UpdateTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                await _container.UpsertItemAsync(timeEntry, new PartitionKey(timeEntry.matterId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating time entry: {ex.Message}");
            }
        }

        // Implement DeleteTimeEntry
        public async Task DeleteTimeEntry(string id, int matterId)
        {
            try
            {
                await _container.DeleteItemAsync<TimeEntry>(id, new PartitionKey(matterId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting time entry: {ex.Message}");
            }
        }
    }
}

