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
        Task<List<Invoice>> GetAllInvoicesByUserId(string userId);
    }
}
