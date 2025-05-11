using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DocumentMGT.DTO
{
    public class DocumentHtmlResponse
    {
        public string? HtmlContent { get; set; }
        public string? DocumentId { get; set; }
        public string? UserId { get; set; }
        public string? FileName { get; set; }

        public string Url { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }

}
