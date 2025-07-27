namespace Aipazz.Application.Billing.DTOs
{
    public class InvoiceDetailsDto
    {
        public string Id { get; set; }=string.Empty;
        public string UserId { get; set; } = string.Empty;

        // Client Info
        public string ClientId { get; set; } = string.Empty;
        public string ClientNic { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientAddress { get; set; } = string.Empty;

        public List<string> MatterIds { get; set; } = new List<string>();
        public List<string> MatterTitles { get; set; } = new List<string>();

        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public List<string> EntryIds { get; set; } = new List<string>();

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public string Status { get; set; } = string.Empty;
        public string FooterNotes { get; set; } = string.Empty;
        public string PaymentProfileNotes { get; set; } = string.Empty;

        public string PdfFileUrl { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string Currency { get; set; } = "Rs.";
        public string Subject { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } = "%";
        public DateTime? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
