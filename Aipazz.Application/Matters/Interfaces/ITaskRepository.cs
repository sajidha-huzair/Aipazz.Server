
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface ITaskRepository
    {
        Task<MatterTask?> GetByIdAsync(string id, string matterId);
        Task<IEnumerable<MatterTask>> GetAllAsync();
        Task<IEnumerable<MatterTask>> GetByMatterIdAsync(string matterId);
        Task<MatterTask?> GetByTitleAsync(string title);
        Task CreateAsync(MatterTask task);
        Task UpdateAsync(MatterTask task);
        Task DeleteAsync(string id, string matterId);
    }
}