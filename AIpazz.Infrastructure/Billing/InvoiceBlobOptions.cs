using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIpazz.Infrastructure.Billing
{
    namespace Aipazz.Application.Common
    {
        public class InvoiceBlobOptions
        {
            public string ConnectionString { get; set; } = string.Empty;
            public string ContainerName { get; set; } = "invoices";
        }
    }

}
