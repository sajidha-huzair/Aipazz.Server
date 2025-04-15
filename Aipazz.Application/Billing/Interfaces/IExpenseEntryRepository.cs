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
        Task<List<ExpenseEntry>> GetAllExpenseEntries();
        Task<ExpenseEntry> GetExpenseEntryById(string id, string matterId);
        Task AddExpenseEntry(ExpenseEntry ExpenseEntry);
        Task UpdateExpenseEntry(ExpenseEntry ExpenseEntry);
        Task DeleteExpenseEntry(string id, string matterId);
    }
}
