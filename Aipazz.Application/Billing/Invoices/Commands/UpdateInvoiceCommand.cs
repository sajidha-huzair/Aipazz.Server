using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    using MediatR;

    public class UpdateInvoiceCommand : IRequest<Unit>   
    {
        public InvoiceDetailsDto Invoice { get; }

        public UpdateInvoiceCommand(InvoiceDetailsDto invoice)
        {
            Invoice = invoice;
        }
    }


}
