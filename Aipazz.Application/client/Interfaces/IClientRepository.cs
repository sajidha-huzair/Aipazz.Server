using Aipazz.Domian.client;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<bool> DoesClientExistByNIC(string nic, string userId);
        Task<List<Client>> GetClientsByTeamIdAsync(string teamId);
    }
}