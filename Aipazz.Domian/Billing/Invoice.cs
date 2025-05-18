using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Billing
{
    public class Invoice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Client & Matter Info
        public string ClientId { get; set; }=string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientAddress { get; set; } = string.Empty;
        public List<string> MatterIds { get; set; } = new();
        public List<string> MatterTitles { get; set; } = new();

        // Invoice Metadata
        public int InvoiceNumber { get; set; } // Auto-increment logic can be handled separately
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }

        // Entry linkage (time/expense entries)
        public List<string> EntryIds { get; set; } = new();

        // Financials
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount => TotalAmount - PaidAmount;

        // Status & Notes
        public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, etc.
        public string FooterNotes { get; set; } = "Please make all amounts payable to: Law Office of {ClientName}";
        public string PaymentProfileNotes { get; set; } = "Please pay within 30 days.";

        // PDF
        public string PdfFileUrl { get; set; } = string.Empty; // link to Azure Blob or local path

        // Timeline / Audit
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }

}
