using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllClients(string userId);
        Task<Client?> GetByNameAsync(string firstName, string lastName, string userId);
        Task<Client?> GetByNicAsync(string nic, string userId);
        Task<Client?> GetByIdAsync(string id, string nic, string userId);
        Task CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id, string nic, string userId);
    }
}