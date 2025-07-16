using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    public interface IExpenseEntryRepository
    {
        Task<List<ExpenseEntry>> GetAllExpenseEntries(string userId);
        Task<ExpenseEntry> GetExpenseEntryById(string id, string matterId, string userId);
        Task AddExpenseEntry(ExpenseEntry ExpenseEntry);
        Task UpdateExpenseEntry(ExpenseEntry ExpenseEntry);
        Task DeleteExpenseEntry(string id, string matterId, string userId);
        Task<List<ExpenseEntry>> GetExpenseEntriesByMatterIdAsync(string matterId, string userId);
        Task<List<ExpenseEntry>> GetAllEntriesByIdsAsync(List<string> entryIds, string userId);
        Task<List<ExpenseEntry>> GetUnbilledByMatterIdAsync(string matterId, string userId);
        Task MarkEntriesInvoicedAsync(IEnumerable<string> ids,string invoiceId,string userId);
        Task<List<ExpenseEntry>> GetBilledByMatterIdAsync(string matterId, string userId);
        Task<ExpenseEntry> UnlinkFromInvoiceAsync(string entryId, string userId);


    }
}
