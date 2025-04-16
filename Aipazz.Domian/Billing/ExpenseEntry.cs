using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Billing
{
    public class ExpenseEntry
    {
        public string id { get; set; } = string.Empty;
        public string matterId { get; set; } = string.Empty; // FK to Matter
        public string Category { get; set; } = string.Empty;  // e.g., Travel
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount => Quantity * Rate;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Draft"; // Optional tabs: Draft, Paid, etc.

    }
}
