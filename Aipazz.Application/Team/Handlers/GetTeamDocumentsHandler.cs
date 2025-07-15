using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.Team.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetTeamDocumentsHandler : IRequestHandler<GetTeamDocumentsQuery, List<Document>>
    {
        private readonly IdocumentRepository _documentRepository;

        public GetTeamDocumentsHandler(IdocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<List<Document>> Handle(GetTeamDocumentsQuery request, CancellationToken cancellationToken)
        {
            return await _documentRepository.GetDocumentsByTeamIdAsync(request.TeamId);
        }
    }
}
