using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    

    public record UpdateInvoiceNotesCommand(
        string InvoiceId,
        string UserId,
        string FooterNotes,
        string PaymentProfileNotes
    ) : IRequest<string>;          // returns the fresh PdfFileUrl

}
