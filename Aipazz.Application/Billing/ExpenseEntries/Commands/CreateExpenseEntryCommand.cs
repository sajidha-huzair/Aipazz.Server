using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using System.ComponentModel;

namespace Aipazz.Application.Billing.ExpenseEntries.Commands
{
    public record CreateExpenseEntryCommand(string MatterId, string Category,int Quantity, string Description,  DateTime Date, decimal Rate, string Status) : IRequest<ExpenseEntry>;

}
