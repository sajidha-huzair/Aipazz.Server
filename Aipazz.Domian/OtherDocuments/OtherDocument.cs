using System;

namespace Aipazz.Domian.OtherDocuments
{
    public class OtherDocument
    {
        public string id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? TeamId { get; set; } = null;
        public string? MatterId { get; set; } = null;
        public string? MatterName { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    }
}