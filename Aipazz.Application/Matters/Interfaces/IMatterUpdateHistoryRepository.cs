using Aipazz.Domian.Matters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IMatterUpdateHistoryRepository
    {
        Task<List<MatterUpdateHistory>> GetMatterUpdateHistory(string matterId, string clientNic, string userId);
        Task AddMatterUpdateHistory(MatterUpdateHistory updateHistory);
    }
}