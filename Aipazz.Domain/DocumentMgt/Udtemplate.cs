using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.DocumentMgt
{
    public class Udtemplate
    {
        public string id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Userid { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public string ContentHtml { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
