using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, Unit>
    {
        private readonly ITeamRepository _repository;

        public DeleteTeamHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteTeamAsync(request.TeamId, request.OwnerId);
            return Unit.Value;
        }
    }
}