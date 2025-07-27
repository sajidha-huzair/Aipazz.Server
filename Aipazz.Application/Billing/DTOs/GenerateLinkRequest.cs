using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{

    public class GenerateLinkRequest
    {
        public string InvoiceId { get; set; }=string.Empty;
    }

}
