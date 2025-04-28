using Aipazz.Domian.Entities;

namespace Aipazz.Application.Interfaces
{
    public interface IClientRepository
    {
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(string id);
        Task<Client> GetClientByIdAsync(string id);
        Task<IEnumerable<Client>> SearchClientsAsync(string searchTerm); // For searching by Name, NIC, or Phone
    }
}