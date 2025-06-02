using Aipazz.Application.Billing.DTOs;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Queries
{
    public class GetInvoiceDetailsQuery : IRequest<InvoiceDetailsDto>
    {
        public string InvoiceId { get; }

        public GetInvoiceDetailsQuery(string invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
