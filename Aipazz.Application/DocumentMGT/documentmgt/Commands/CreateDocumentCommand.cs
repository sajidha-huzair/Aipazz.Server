using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.DTO;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    public class CreateDocumentCommand : IRequest<string>
    {
        public CreateDocumentRequest Request { get; set; }
        public CreateDocumentCommand (CreateDocumentRequest request)
        {
            Request = request;
        }
    }
}
