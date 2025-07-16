using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace AIpazz.Infrastructure.Calendar
{
    public class FilingsDeadlineFormRepository : IFilingsDeadlineFormRepository
    {
        private readonly Container _container;

        public FilingsDeadlineFormRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["FilingsDeadlineForm"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<FilingsDeadlineForm>> GetAll()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<FilingsDeadlineForm>(query);
            var results = new List<FilingsDeadlineForm>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching filings deadlines: {ex.Message}");
                }
            }

            return results;
        }

        public async Task<FilingsDeadlineForm?> GetById(Guid id)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id.ToString());

                var iterator = _container.GetItemQueryIterator<FilingsDeadlineForm>(query);

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    var form = response.FirstOrDefault();
                    if (form != null)
                        return form;
                }

                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task Add(FilingsDeadlineForm form)
        {
            try
            {
                await _container.CreateItemAsync(form, new PartitionKey(form.PartitionKey));
                Console.WriteLine($"Successfully added filing deadline ID: {form.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding filing deadline: {ex.Message}");
                throw;
            }
        }

        public async Task<FilingsDeadlineForm?> Update(Guid id, FilingsDeadlineForm updatedForm)
        {
            try
            {
                var existing = await GetById(id);
                if (existing == null)
                {
                    return null;
                }

                existing.Title = updatedForm.Title;
                existing.Date = updatedForm.Date;
                existing.Time = updatedForm.Time;
                existing.Reminder = updatedForm.Reminder;
                existing.Description = updatedForm.Description;
                existing.AssignedMatter = updatedForm.AssignedMatter;

                await _container.UpsertItemAsync(existing, new PartitionKey(existing.PartitionKey));
                return existing;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating filing deadline: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var existing = await GetById(id);
                if (existing == null)
                    return false;

                await _container.DeleteItemAsync<FilingsDeadlineForm>(existing.id, new PartitionKey(existing.PartitionKey));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting filing deadline: {ex.Message}");
                throw;
            }
        }
    }
}
