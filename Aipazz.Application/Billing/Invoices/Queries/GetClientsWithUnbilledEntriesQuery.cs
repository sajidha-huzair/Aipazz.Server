using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Queries
{
    public class GetClientsWithUnbilledEntriesQuery
    : IRequest<List<ClientWithMattersDto>>
    {
        public string UserId { get; }
        public GetClientsWithUnbilledEntriesQuery(string userId) => UserId = userId;
    }

}
