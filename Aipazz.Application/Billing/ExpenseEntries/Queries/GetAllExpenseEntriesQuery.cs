using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Queries
{
    public record GetAllExpenseEntriesQuery() : IRequest<List<ExpenseEntry>>;
}
