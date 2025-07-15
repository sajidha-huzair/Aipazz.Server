using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    public interface IInvoicePdfGenerator
    {
        Task<byte[]> GeneratePdfAsync(Invoice invoice, List<TimeEntry> timeEntries, List<ExpenseEntry> expenseEntries);
    }

}
