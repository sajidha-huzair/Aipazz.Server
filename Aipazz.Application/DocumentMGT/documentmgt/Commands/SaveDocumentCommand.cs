using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    public class SaveDocumentCommand:IRequest<string>
    {
        public string Name { get; set; }
        public string ContentHtml { get; set; }
    }
}
