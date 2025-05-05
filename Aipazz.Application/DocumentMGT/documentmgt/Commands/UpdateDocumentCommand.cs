using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.DTO;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    public class UpdateDocumentCommand : IRequest<bool>
    {
        public UpdateDocumentRequest Request { get; }
        public UpdateDocumentCommand(UpdateDocumentRequest request)
        {
            Request = request;
        }
    }
}
