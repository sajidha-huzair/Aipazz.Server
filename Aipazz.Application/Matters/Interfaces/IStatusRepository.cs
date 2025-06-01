using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetAllStatuses(string userId);
        Task<Status?> GetStatusById(string id, string userId); 
        Task AddStatus(Status status);
        Task UpdateStatus(Status status);
        Task DeleteStatus(string id, string userId);
        Task<Status?> GetStatusByName(string name);

    }
}
