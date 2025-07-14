using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, string>
    {
        private readonly ITeamRepository _repository;

        public CreateTeamHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Aipazz.Domian.Team.Team
            {
                id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.UserId,
                Members = request.Members ?? new List<TeamMember>(),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _repository.CreateTeamAsync(team);
        }
    }
}