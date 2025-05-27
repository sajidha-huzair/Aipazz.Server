using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Aipazz.Application.Billing.DTOs;

namespace Aipazz.Application.Billing.ExpenseEntries.Commands
{
    public class CreateExpenseEntryCommand : IRequest<ExpenseEntryDto>
    {
        public string UserId { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal Rate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}