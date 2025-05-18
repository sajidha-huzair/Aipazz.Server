using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aipazz.Application.Billing.ExpenseEntries.Commands
{
    public class UpdateExpenseEntryCommand : IRequest<ExpenseEntryDto>
    {
        public string Id { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Draft"; // Optional tabs: Draft, Paid, etc.

    }
}

