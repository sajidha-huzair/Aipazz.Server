using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Interfaces
{
    public interface IClientRepository
    {
<<<<<<< Updated upstream
        Task<List<Client>> GetAllClients();
        Task<Client?> GetByNameAsync(string name);
        Task<Client?> GetByNICAsync(string nic);
        Task CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id);
=======
        Task<List<Client>> GetAllClients(string userId);
        Task<Client?> GetByNameAsync(string firstName, string lastName, string userId);
        Task<Client?> GetByNicAsync(string nic, string userId);
        Task<Client?> GetByIdAsync(string id, string nic, string userId);
        Task CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id, string nic, string userId);
>>>>>>> Stashed changes
    }
}