using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using Microsoft.Azure.Cosmos;

namespace AIpazz.Infrastructure.Documentmgt
{
    public class UdtemplateRepository : IUdtemplateRepository
    {
        private readonly Container _container;

        public UdtemplateRepository(CosmosClient client)
        {
            _container = client.GetContainer("Aipazz", "UdTemplate");
        }

        public async Task CreateTemplate(Udtemplate Udtemplate)
        {
            await _container.CreateItemAsync(Udtemplate, new PartitionKey(Udtemplate.Userid));
        }

        public async Task DeleteTemplate(string id)
        {
            // This method signature matches the interface but lacks user filtering
            // For security, you should consider updating the interface to include userId
            await _container.DeleteItemAsync<Udtemplate>(id, new PartitionKey(id));
        }

        public async Task DeleteTemplate(string id, string userId)
        {
            await _container.DeleteItemAsync<Udtemplate>(id, new PartitionKey(userId));
        }

        public async Task<List<Udtemplate>> GetAllTemplates()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Udtemplate>(query);
            List<Udtemplate> templates = new List<Udtemplate>();
            
            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    templates.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching all templates: {ex.Message}");
                }
            }
            return templates;
        }

        public async Task<List<Udtemplate>> GetAllTemplatesByUserId(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Userid = @userId")
                .WithParameter("@userId", userId);
            
            var iterator = _container.GetItemQueryIterator<Udtemplate>(query);
            List<Udtemplate> templates = new List<Udtemplate>();
            
            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    templates.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching templates for user {userId}: {ex.Message}");
                }
            }
            return templates;
        }

        public async Task<Udtemplate?> GetTemplateById(string id)
        {
            try
            {
                // This approach requires knowing the partition key, which is problematic
                // You should consider updating the interface to include userId
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id);
                
                var iterator = _container.GetItemQueryIterator<Udtemplate>(query);
                
                if (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    return response.FirstOrDefault();
                }
                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Udtemplate?> GetTemplateByIdAndUserId(string id, string userId)
        {
            try
            {
                ItemResponse<Udtemplate> response = await _container.ReadItemAsync<Udtemplate>(id, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateTemplate(Udtemplate Udtemplate)
        {
            await _container.UpsertItemAsync(Udtemplate, new PartitionKey(Udtemplate.Userid));
        }
    }
}
