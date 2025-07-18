using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;

namespace Aipazz.Application.Billing.Interfaces
{
        public interface ITokenRepository
        {
            Task SaveTokenAsync(InvoiceAccessToken token);
            Task<InvoiceAccessToken?> GetTokenAsync(string token);
            Task InvalidateTokenAsync(string token);
        }
    }


