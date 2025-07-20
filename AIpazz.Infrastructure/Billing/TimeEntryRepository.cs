using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian.Billing;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;

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
        public async Task<List<TimeEntry>> GetAllTimeEntries(string userId)
        {
            var query = _container.GetItemLinqQueryable<TimeEntry>(allowSynchronousQueryExecution: false)
                                  .Where(t => t.UserId == userId)
                                  .AsQueryable();

            var iterator = query.ToFeedIterator();
            var results = new List<TimeEntry>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<TimeEntry> GetTimeEntryById(string id, string matterId, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<TimeEntry>(id, new PartitionKey(matterId));
                return response.Resource?.UserId == userId ? response.Resource : null;
            }
            catch
            {
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
            var existing = await GetTimeEntryById(timeEntry.id, timeEntry.matterId, timeEntry.UserId);
            if (existing == null) return;

            await _container.UpsertItemAsync(timeEntry, new PartitionKey(timeEntry.matterId));
        }


        // Implement DeleteTimeEntry
        public async Task DeleteTimeEntry(string id, string matterId, string userId)
        {
            var entry = await GetTimeEntryById(id, matterId, userId);
            if (entry != null)
            {
                await _container.DeleteItemAsync<TimeEntry>(id, new PartitionKey(matterId));
            }
        }


        public async Task<List<TimeEntry>> GetTimeEntriesByMatterIdAsync(string matterId, string userId)
        {
            var query = _container.GetItemLinqQueryable<TimeEntry>(allowSynchronousQueryExecution: false)
                                  .Where(t => t.matterId == matterId && t.UserId == userId)
                                  .ToFeedIterator();

            var timeEntries = new List<TimeEntry>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                timeEntries.AddRange(response);
            }

            return timeEntries;
        }

        public async Task<List<TimeEntry>> GetAllEntriesByIdsAsync(List<string> entryIds, string userId)
        {
            var query = _container.GetItemLinqQueryable<TimeEntry>(true)
                .Where(e => entryIds.Contains(e.id) && e.UserId == userId)
                .ToFeedIterator();

            var results = new List<TimeEntry>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
        //Fetch only unbilled entries (for UI list)
        public async Task<List<TimeEntry>> GetUnbilledByMatterIdAsync(string matterId, string userId)
        {
            var query = _container.GetItemLinqQueryable<TimeEntry>(
                            requestOptions: new QueryRequestOptions
                            {
                                PartitionKey = new PartitionKey(matterId)   // ← FIX HERE
                            })
                        .Where(e => e.matterId == matterId &&
                                    e.UserId == userId &&
                                    e.InvoiceId == null)
                        .ToFeedIterator();

            var results = new List<TimeEntry>();
            while (query.HasMoreResults)
                results.AddRange(await query.ReadNextAsync());
            return results;
        }


        public async Task MarkEntriesInvoicedAsync(IEnumerable<string> entryIds,
                                           string invoiceId,
                                           string userId)
        {
            //  Pull only the entries we need (so we know their matterId)
            var query = _container.GetItemLinqQueryable<TimeEntry>(true)
                                  .Where(e => entryIds.Contains(e.id) && e.UserId == userId)
                                  .ToFeedIterator();

            var entries = new List<TimeEntry>();
            while (query.HasMoreResults)
                entries.AddRange(await query.ReadNextAsync());

            //  Update & replace using the partition key (matterId)
            foreach (var entry in entries)
            {
                entry.InvoiceId = invoiceId;
                await _container.ReplaceItemAsync(entry,
                                                  entry.id,
                                                  new PartitionKey(entry.matterId)); 
            }
        }


        public async Task<List<TimeEntry>> GetBilledByMatterIdAsync(string matterId,
                                                            string userId)
        {
            var iterator = _container.GetItemLinqQueryable<TimeEntry>(
                                requestOptions: new QueryRequestOptions
                                {
                                    PartitionKey = new PartitionKey(matterId)   // ← use matterId PK
                                })
                            .Where(e => e.UserId == userId          // still scoped to owner
                                     && e.InvoiceId != null)        // billed only
                            .ToFeedIterator();

            var results = new List<TimeEntry>();
            while (iterator.HasMoreResults)
                results.AddRange(await iterator.ReadNextAsync());

            return results;
        }

        // Unlink time entry from invoice (move back to Draft)
        public async Task<TimeEntry> UnlinkFromInvoiceAsync(string entryId, string userId)
        {
            // First, find the entry by scanning all partitions since we don't know the matterId
            var query = _container.GetItemLinqQueryable<TimeEntry>(true)
                                  .Where(e => e.id == entryId && e.UserId == userId)
                                  .ToFeedIterator();

            TimeEntry entry = null;
            while (query.HasMoreResults && entry == null)
            {
                var response = await query.ReadNextAsync();
                entry = response.FirstOrDefault();
            }

            if (entry == null)
                throw new Exception($"Time entry with ID {entryId} not found for user {userId}");

            // Clear the invoice ID to unlink it
            entry.InvoiceId = null;

            // Update the entry in the database
            await _container.ReplaceItemAsync(entry, entry.id, new PartitionKey(entry.matterId));

            Console.WriteLine($"Successfully unlinked time entry ID: {entryId} from invoice");

            return entry;
        }



    }
}

