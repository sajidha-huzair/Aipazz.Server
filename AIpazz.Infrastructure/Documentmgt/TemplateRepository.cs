using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.DocumentMgt;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AIpazz.Infrastructure.Documentmgt
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public TemplateRepository(CosmosClient client,IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["Template"];
            _container = db.GetContainer(containerName);

        }

        public async Task CreateTemplate(Template template)
        {
            await _container.CreateItemAsync(template, new PartitionKey(template.id));
        }

        public async Task DeleteTemplate(string id)
        {
            await _container.DeleteItemAsync<Template>(id,new PartitionKey(id));
        }

        public async Task<List<Template>> GetAllTemplates()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Template>(query);
            List<Template> templates = new List<Template>();
            while (iterator.HasMoreResults)
            {
                try
                {
                    var response = await iterator.ReadNextAsync();
                    templates.AddRange(response);
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"Error fetching templates: {ex.Message}");
                }
            }
            return templates;
        }

        public async Task<Template?> GetTemplateById(string id)
        {
            try {
                ItemResponse<Template> response = await _container.ReadItemAsync<Template>(id, new PartitionKey(id));
                return response.Resource;

            
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound ) {
                return null;
            }
        }

        public async Task UpdateTemplate(Template template)
        {
            await _container.UpsertItemAsync(template, new PartitionKey(template.id)); ;
        }
    }
}
