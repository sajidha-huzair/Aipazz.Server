using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class GetTeamSharedDocumentsHandler : IRequestHandler<GetTeamSharedDocumentsQuery, List<Document>>
    {
        private readonly IdocumentRepository _documentRepository;
        private readonly ITeamRepository _teamRepository;

        public GetTeamSharedDocumentsHandler(IdocumentRepository documentRepository, ITeamRepository teamRepository)
        {
            _documentRepository = documentRepository;
            _teamRepository = teamRepository;
        }

        public async Task<List<Document>> Handle(GetTeamSharedDocumentsQuery request, CancellationToken cancellationToken)
        {
            // Get all teams the user is part of
            var userTeams = await _teamRepository.GetTeamsByUserIdAsync(request.UserId);
            var teamIds = userTeams.Select(t => t.id).ToList();

            // Get all documents shared with these teams
            var sharedDocuments = await _documentRepository.GetDocumentsByTeamIdsAsync(teamIds);
            
            return sharedDocuments;
        }
    }
}