using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DocumentMGT.DTO
{
    public class CreateDocumentRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ContentHtml { get; set; } = string.Empty;
    }
}
