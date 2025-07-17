using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class GetDocumentsByMatterIdHandler : IRequestHandler<GetDocumentsByMatterIdQuery, List<Document>>
    {
        private readonly IdocumentRepository _repository;

        public GetDocumentsByMatterIdHandler(IdocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Document>> Handle(GetDocumentsByMatterIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDocumentsByMatterIdAsync(request.MatterId, request.UserId);
        }
    }
}