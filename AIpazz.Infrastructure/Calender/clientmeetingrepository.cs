using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace AIpazz.Infrastructure.Calender
{
    public class Clientmeetingrepository : IclientmeetingRepository
    {
        private readonly Container _container;

        public Clientmeetingrepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["ClientMeeting"];
            _container = db.GetContainer(containerName);
        }

        public async Task<ClientMeeting> GetClientMeetingByID(Guid id)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id.ToString());

                var iterator = _container.GetItemQueryIterator<ClientMeeting>(query);

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    var meeting = response.FirstOrDefault();
                    if (meeting != null)
                        return meeting;
                }

                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddClientMeeting(ClientMeeting meeting)
        {
            try
            {
                await _container.CreateItemAsync(meeting, new PartitionKey(meeting.PartitionKey));
                Console.WriteLine($"Successfully added client meeting ID: {meeting.id}");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error adding client meeting: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ClientMeeting>> GetAllClientMeetings()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<ClientMeeting>(query);
            var results = new List<ClientMeeting>();

            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching client meetings: {ex.Message}");
                }
            }

            return results;
        }

        public async Task<ClientMeeting> UpdateClientMeeting(ClientMeeting meeting)
        {
            try
            {
                var existing = await GetClientMeetingByID(meeting.Id);
                if (existing == null)
                {
                    return null;
                }

                await _container.UpsertItemAsync(meeting, new PartitionKey(meeting.PartitionKey));
                return meeting;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating client meeting: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteClientMeeting(Guid id)
        {
            try
            {
                var existing = await GetClientMeetingByID(id);
                if (existing == null)
                    return false;

                await _container.DeleteItemAsync<ClientMeeting>(existing.id, new PartitionKey(existing.PartitionKey));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting client meeting: {ex.Message}");
                throw;
            }
        }
    }
}
