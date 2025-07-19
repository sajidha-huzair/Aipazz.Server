using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public record GenerateInvoiceAccessLinkCommand(string UserId, string InvoiceId) : IRequest<string>;

}
