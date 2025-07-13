using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Queries
{
    public class GetAllInvoicesForUserQuery : IRequest<List<InvoiceListDto>>
    {
        public string UserId { get; set; }

        public GetAllInvoicesForUserQuery(string userId)
        {
            UserId = userId;
        }
    }

}
