using System;

namespace Aipazz.Domian.Notification
{
    public class Notification
    {
        public string id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;// Who receives it
        public string? RecipientEmail { get; set; } // Add this
        public string Type { get; set; } = string.Empty;          // "TeamCreated", "TeamAssignment", etc.
        public string Title { get; set; } = string.Empty;         // "New Team Created"
        public string Message { get; set; } = string.Empty;       // Detailed message
        public string? RelatedEntityId { get; set; }              // TeamId, DocumentId, etc.
        public string? RelatedEntityType { get; set; }            // "Team", "Document"
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ActionUrl { get; set; }                    // Deep link to relevant page
        public string? CreatedBy { get; set; }                    // Who triggered the notification
        public string? InvoiceId { get; set; }
    }
}