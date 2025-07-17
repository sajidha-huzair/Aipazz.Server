using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class InvoiceListDto
    {
        public string Id { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string MatterSummary { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsSent { get; set; } // You can customize based on status
    }
}
