using Aipazz.Domian.Matters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IMatterRepository
    {
        Task<List<Matter>> GetAllMatters();
        Task<Matter> GetMatterById(string id, string clientNic); // 'title' is the partition key
        Task AddMatter(Matter Matter);
        Task UpdateMatter(Matter Matter);
        Task DeleteMatter(string id, string clientNic); // title is used to identify the partition
        Task<List<Matter>> GetMattersByClientNicAsync(string ClientNic);

    }
}
