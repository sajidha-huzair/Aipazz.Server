using System;

namespace Aipazz.Domian.DocumentMgt
{
    public class Document
    {
        public string id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Userid { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public string? TeamId { get; set; } = null; // Added nullable TeamId
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
