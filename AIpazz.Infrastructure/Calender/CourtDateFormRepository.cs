using Aipazz.Domian.Calender;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;
using Aipazz.Application.Calender.Interface;

namespace Aipazz.Infrastructure.Calender
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

        public async Task<List<CourtDateForm>> GetAll(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

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

        public async Task<CourtDateForm> AddCourtDateForm(CourtDateForm courtDateForm)
        {
            try
            {
                await _container.CreateItemAsync(courtDateForm, new PartitionKey(courtDateForm.PartitionKey));
                Console.WriteLine($"Successfully added court date ID: {courtDateForm.Id}");
                return courtDateForm;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding court date: {ex.Message}");
                throw;
            }
        }

        public async Task<CourtDateForm?> UpdateCourtDateForm(Guid id, CourtDateForm courtDateForm)
        {
            try
            {
                var existing = await GetById(id);
                if (existing == null)
                {
                    return null;
                }
                existing.Title = courtDateForm.Title;
                existing.CourtType = courtDateForm.CourtType;
                existing.Stage = courtDateForm.Stage;
                existing.Clients = courtDateForm.Clients;
                existing.CourtDate = courtDateForm.CourtDate;
                existing.Reminder = courtDateForm.Reminder;
                existing.Note = courtDateForm.Note;
                existing.TeamMembers = courtDateForm.TeamMembers;
                existing.ClientEmails = courtDateForm.ClientEmails;
                await _container.UpsertItemAsync(existing, new PartitionKey(existing.PartitionKey));
                return existing;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating court date: {ex.Message}");
                throw;
            }
        }

        public async Task<CourtDateForm?> DeleteCourtDateForm(Guid id)
        {
            try
            {
                var existing = await GetById(id);
                if (existing is null)
                    return null;

                await _container.DeleteItemAsync<CourtDateForm>(existing.Id.ToString(), new PartitionKey(existing.PartitionKey));
                return existing;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting court date: {ex.Message}");
                throw;
            }
        }
    }
}
