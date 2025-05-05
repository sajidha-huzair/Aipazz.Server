using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllClients();
        Task<Client?> GetByNameAsync(string name);
        Task<Client?> GetByNICAsync(string nic);
        Task CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id);
    }
}