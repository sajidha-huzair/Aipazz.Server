using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class GenerateInvoiceFromSelectionCommand : IRequest<string>
    {
        public string UserId { get; set; } = string.Empty;
        public string ClientNic { get; set; } = string.Empty;
        public List<string> EntryIds { get; set; } = new(); // time + expense IDs selected
        public bool Force { get; set; } = false;

    }

}
