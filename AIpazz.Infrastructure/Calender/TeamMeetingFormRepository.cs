using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace AIpazz.Infrastructure.Calender
{
    public class TeamMeetingFormRepository : ITeamMeetingFormRepository
    {
        private readonly Container _container;

        public TeamMeetingFormRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["TeamMeetingForm"];
            _container = db.GetContainer(containerName);
        }

        public async Task<List<TeamMeetingForm>> GetAll(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<TeamMeetingForm>(query);
            var results = new List<TeamMeetingForm>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching team meetings: {ex.Message}");
                }
            }

            return results;
        }


        public async Task<TeamMeetingForm?> GetById(Guid id)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id.ToString());

                var iterator = _container.GetItemQueryIterator<TeamMeetingForm>(query);

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

        public async Task Add(TeamMeetingForm form)
        {
            try
            {
                await _container.CreateItemAsync(form, new PartitionKey(form.PartitionKey));
                Console.WriteLine($"Successfully added team meeting ID: {form.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding team meeting: {ex.Message}");
                throw;
            }
        }

        public async Task<TeamMeetingForm?> Update(Guid id, TeamMeetingForm updatedForm)
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
                existing.VideoConferencingLink = updatedForm.VideoConferencingLink;
                existing.LocationLink = updatedForm.LocationLink;
                existing.TeamMembers = updatedForm.TeamMembers;

                await _container.UpsertItemAsync(existing, new PartitionKey(existing.PartitionKey));
                return existing;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating team meeting: {ex.Message}");
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

                await _container.DeleteItemAsync<TeamMeetingForm>(existing.id, new PartitionKey(existing.PartitionKey));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting team meeting: {ex.Message}");
                throw;
            }
        }
    }
}
