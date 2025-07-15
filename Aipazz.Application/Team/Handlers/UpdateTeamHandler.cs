using System;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, Unit>
    {
        private readonly ITeamRepository _repository;

        public UpdateTeamHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            request.Team.LastModifiedAt = DateTime.UtcNow;
            await _repository.UpdateTeamAsync(request.Team);
            return Unit.Value;
        }
    }
}