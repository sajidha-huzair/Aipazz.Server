using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllClients();
        Task<Client?> GetByNameAsync(string firstName, string lastName);
        Task<Client?> GetByNicAsync(string nic);
        Task<Client?> GetByIdAsync(string id, string nic);
        Task CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id, string nic);
    }
}