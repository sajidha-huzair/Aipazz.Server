using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.DTO;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Queries
{
   public class GetDocumentByIdQuery : IRequest<DocumentHtmlResponse?>
    {
        public string UserId { get; set; }
        public string DocumentId { get; set; }
       


        public GetDocumentByIdQuery(string documentId,string userId)
        {
            UserId = userId;
            DocumentId = documentId;
            
            
        }

    }
}
