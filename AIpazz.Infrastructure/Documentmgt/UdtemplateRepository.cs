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

        public Task DeleteTemplate(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Udtemplate>> GetAllTemplates()
        {
            throw new NotImplementedException();
        }

        public Task<Udtemplate?> GetTemplateById(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTemplate(Udtemplate Udtemplate)
        {
            throw new NotImplementedException();
        }
    }
}
