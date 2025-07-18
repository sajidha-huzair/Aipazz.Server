using System;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class RemoveDocumentFromTeamHandler : IRequestHandler<RemoveDocumentFromTeamCommand, bool>
    {
        private readonly IdocumentRepository _repo;

        public RemoveDocumentFromTeamHandler(IdocumentRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(RemoveDocumentFromTeamCommand command, CancellationToken cancellationToken)
        {
            var document = await _repo.GetByIdAsync(command.DocumentId, command.UserId);

            if (document == null)
                return false;

            // Check if document is actually shared with a team
            if (string.IsNullOrEmpty(document.TeamId))
                return false; // Document is not shared with any team

            // Remove team assignment
            document.TeamId = null;
            document.LastModifiedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(document);
            return true;
        }
    }
}