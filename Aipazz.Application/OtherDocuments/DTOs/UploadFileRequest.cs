using Microsoft.AspNetCore.Http;

namespace Aipazz.Application.OtherDocuments.DTOs
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; } = null!;
        public string? MatterId { get; set; }
        public string? MatterName { get; set; }
    }
}