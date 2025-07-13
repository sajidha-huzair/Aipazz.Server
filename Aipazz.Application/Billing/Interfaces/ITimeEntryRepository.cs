using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task<List<TimeEntry>> GetAllTimeEntries(string userId);
        Task<TimeEntry> GetTimeEntryById(string id, string matterId, string userId);
        Task AddTimeEntry(TimeEntry timeEntry);
        Task UpdateTimeEntry(TimeEntry timeEntry);
        Task DeleteTimeEntry(string id, string matterId, string userId);
        Task<List<TimeEntry>> GetTimeEntriesByMatterIdAsync(string matterId, string userId);
        Task<List<TimeEntry>> GetAllEntriesByIdsAsync(List<string> entryIds, string userId);
        Task<List<TimeEntry>> GetUnbilledByMatterIdAsync(string matterId, string userId);
        Task MarkEntriesInvoicedAsync(IEnumerable<string> ids, string invoiceId, string userId);
        Task<List<TimeEntry>> GetBilledByMatterIdAsync(string matterId, string userId);

    }
}
