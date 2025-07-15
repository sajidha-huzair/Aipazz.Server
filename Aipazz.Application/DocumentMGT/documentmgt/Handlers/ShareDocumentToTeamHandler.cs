using System;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.Team.Interfaces;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class ShareDocumentToTeamHandler : IRequestHandler<ShareDocumentToTeamCommand, Unit>
    {
        private readonly IdocumentRepository _documentRepository;
        private readonly ITeamRepository _teamRepository;

        public ShareDocumentToTeamHandler(IdocumentRepository documentRepository, ITeamRepository teamRepository)
        {
            _documentRepository = documentRepository;
            _teamRepository = teamRepository;
        }

        public async Task<Unit> Handle(ShareDocumentToTeamCommand request, CancellationToken cancellationToken)
        {
            // Verify the document exists and user owns it
            var document = await _documentRepository.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null)
                throw new KeyNotFoundException("Document not found or you don't have permission to share it.");

            // Verify the team exists and user is a member/owner
            var team = await _teamRepository.GetTeamByIdAsync(request.TeamId, request.UserId);
            if (team == null)
                throw new KeyNotFoundException("Team not found or you're not a member of this team.");

            // Update document with TeamId
            document.TeamId = request.TeamId;
            document.LastModifiedAt = DateTime.UtcNow;

            await _documentRepository.UpdateAsync(document);
            return Unit.Value;
        }
    }
}