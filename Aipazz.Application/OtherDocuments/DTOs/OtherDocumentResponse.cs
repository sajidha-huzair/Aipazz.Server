namespace Aipazz.Application.OtherDocuments.DTOs
{
    public class OtherDocumentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? MatterId { get; set; }
        public string? MatterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}