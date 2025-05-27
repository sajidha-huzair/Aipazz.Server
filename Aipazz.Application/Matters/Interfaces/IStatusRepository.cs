using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetAllStatuses();
        Task<Status?> GetStatusById(string id, string Name); // Name = partition key
        Task AddStatus(Status status);
        Task UpdateStatus(Status status);
        Task DeleteStatus(string id, string Name); // name = partition key
    }
}
