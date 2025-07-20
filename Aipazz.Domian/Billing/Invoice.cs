using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Billing
{
    public class Invoice
    {
        public string id { get; set; } = Guid.NewGuid().ToString();

        // Authorization
        public string UserId { get; set; } = string.Empty;

        // Client Info (denormalized for fast access)
        public string ClientId { get; set; } = string.Empty;
        public string ClientNic { get; set; } = string.Empty; // for cross-joins / filters
        public string ClientName { get; set; } = string.Empty;
        public string ClientAddress { get; set; } = string.Empty;

        
        public List<string> MatterIds { get; set; } = new();
        public List<string> MatterTitles { get; set; } = new();

        // Invoice Metadata
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }

        // Entry linkage (Time/Expense)
        public List<string> EntryIds { get; set; } = new();

        // Financials
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount => TotalAmount - PaidAmount;

        // Status & Notes
        public string Status { get; set; } = "Draft";
        public string FooterNotes { get; set; } = "Please make all amounts payable to: Law Office of {UserName}";
        public string PaymentProfileNotes { get; set; } = "Please pay within 30 days.";

        // PDF
        public string PdfFileUrl { get; set; } = string.Empty;

        // Audit
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }

        public string Currency { get; set; } = "Rs.";      // or "LKR (Rs)"
        public string Subject { get; set; } = string.Empty;

        /// <summary> If DiscountType is "%", treat value as percentage; if "Rs", treat as fixed amount. </summary>
        public decimal DiscountValue { get; set; } = 0m;
        public string DiscountType { get; set; } = "%";
        public DateTime? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
    }


}
