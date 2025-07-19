using Aipazz.Domian.Matters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IMatterRepository
    {
        Task<List<Matter>> GetAllMatters(string userId);
        Task<Matter?> GetMatterById(string id, string clientNic, string userId); 
        Task AddMatter(Matter Matter);
        Task UpdateMatter(Matter Matter);
        Task DeleteMatter(string id, string clientNic, string userId); 
        Task<List<Matter>> GetMattersByClientNicAsync(string ClientNic, string userId);
        Task<List<Matter>> GetMattersByIdsAsync(List<string> matterIds, string userId);
       
    }
}
