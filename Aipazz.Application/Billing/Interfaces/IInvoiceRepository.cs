using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    public interface IInvoiceRepository
    {
        Task CreateAsync(Invoice invoice);
        Task<Invoice?> GetLastInvoiceAsync(string userId);
        Task<Invoice?> GetByIdAsync(string id, string userId);
        Task<List<Invoice>> GetAllForUserAsync(string userId);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(string id, string userId);



    }
}
