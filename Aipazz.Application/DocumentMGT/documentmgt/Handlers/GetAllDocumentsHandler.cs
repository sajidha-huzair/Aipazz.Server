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
    public class GetAllDocumentsHandler : IRequestHandler<GetAllDcoumentsQuery, List<Document>>
    {
        private readonly IdocumentRepository _repository;

        public GetAllDocumentsHandler(IdocumentRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Document>> Handle(GetAllDcoumentsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllDocuments();
        }
    }
}
