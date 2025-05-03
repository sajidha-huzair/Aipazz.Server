using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Commands
{
    
        public record DeleteDocumentCommand(string DocumentId, string UserId) : IRequest<bool>;
    
}
