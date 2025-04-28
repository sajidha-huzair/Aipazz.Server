using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Queries
{
    public class GenerateWordFromHtmlQuery : IRequest<byte[]>
    {
        public string HtmlContent { get; set; }

        public GenerateWordFromHtmlQuery(string html)
        {
            HtmlContent = html;
        }
    }
}
