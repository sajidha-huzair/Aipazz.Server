using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task<List<TimeEntry>> GetAllTimeEntries();
        Task<TimeEntry> GetTimeEntryById(string id, string matterId);
        Task AddTimeEntry(TimeEntry timeEntry);
        Task UpdateTimeEntry(TimeEntry timeEntry);
        Task DeleteTimeEntry(string id, string matterId);
    }
}
