using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Queries
{
    public class GetExpenseEntryByIdQuery: IRequest<ExpenseEntry>
    {
        public string Id { get; set; }
        public string MatterId { get; set; }

        public GetExpenseEntryByIdQuery(string id, string matterId)
        {
            Id = id;
            MatterId = matterId;
        }
    }
}
