using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace Aipazz.Infrastructure.Calendar
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        private readonly Container _container;
        
        public CourtDateFormRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["CourtDateForm"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<CourtDateForm>> GetAll()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<CourtDateForm>(query);
            var courtDates = new List<CourtDateForm>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    courtDates.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching court dates: {ex.Message}");
                }
            }

            return courtDates;
        }

        public async Task<CourtDateForm?> GetById(Guid id)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id.ToString());
                
                var iterator = _container.GetItemQueryIterator<CourtDateForm>(query);
                
                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    var courtDate = response.FirstOrDefault();
                    if (courtDate != null)
                        return courtDate;
                }
                
                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddCourtDateForm(CourtDateForm courtDateForm)
        {
            try
            {
                await _container.CreateItemAsync(courtDateForm, new PartitionKey(courtDateForm.PartitionKey));
                Console.WriteLine($"Successfully added court date ID: {courtDateForm.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding court date: {ex.Message}");
                throw;
            }
        }

        public async Task<CourtDateForm> UpdateCourtDateForm(Guid modelId, CourtDateForm courtDateForm)
        {
            try
            {
                var existing = await GetById(modelId);
                if (existing == null)
                {
                    return null;
                }

                existing.CaseNumber = courtDateForm.CaseNumber;
                existing.CourtName = courtDateForm.CourtName;
                existing.Date = courtDateForm.Date;
                existing.Description = courtDateForm.Description;

                await _container.UpsertItemAsync(existing, new PartitionKey(existing.PartitionKey));
                return existing;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating court date: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCourtDateForm(Guid id)
        {
            try
            {
                var existing = await GetById(id);
                if (existing == null)
                    return false;

                await _container.DeleteItemAsync<CourtDateForm>(existing.id, new PartitionKey(existing.PartitionKey));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting court date: {ex.Message}");
                throw;
            }
        }
    }
}
