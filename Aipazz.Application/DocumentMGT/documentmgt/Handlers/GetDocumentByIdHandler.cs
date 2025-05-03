using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, Document?>
    {
        private readonly IdocumentRepository _repository;
        public GetDocumentByIdHandler(IdocumentRepository repository)
        {
            _repository = repository;
            
        }
        public async Task<Document?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.DocumentId, request.UserId);
        }
    }
}
