using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Commands
{
    public class DeleteExpenseEntryCommand: IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
