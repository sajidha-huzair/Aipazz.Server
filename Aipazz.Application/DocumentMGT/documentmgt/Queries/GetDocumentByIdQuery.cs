using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Queries
{
   public class GetDocumentByIdQuery : IRequest<Document?>
    {
        public string DocumentId { get; set; }
        public string UserId { get; set; }


        public GetDocumentByIdQuery(string documentId,string userId)
        {

            DocumentId = documentId;
            UserId = userId;
            
        }

    }
}
