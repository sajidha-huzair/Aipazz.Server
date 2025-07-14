using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class DeleteTeamMemberHandler : IRequestHandler<DeleteTeamMemberCommand, Unit>
    {
        private readonly ITeamRepository _repository;

        public DeleteTeamMemberHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteTeamMemberAsync(request.TeamId, request.MemberId, request.UserId);
            return Unit.Value;
        }
    }
}