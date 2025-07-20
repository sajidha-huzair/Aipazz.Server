using System;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.DTOs;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class AddTeamMemberHandler : IRequestHandler<AddTeamMemberCommand, TeamMemberDto>
    {
        private readonly ITeamRepository _repository;

        public AddTeamMemberHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<TeamMemberDto> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var member = new TeamMember
            {
                Id = Guid.NewGuid().ToString(),
                TeamId = request.TeamId,
                UserId = request.Member.UserId,
                Email = request.Member.Email,
                FirstName = request.Member.FirstName,
                LastName = request.Member.LastName,
                Role = request.Member.Role,
                JoinedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Use request.UserId instead of string.Empty
            var addedMember = await _repository.AddTeamMemberAsync(request.TeamId, member, request.UserId);

            return new TeamMemberDto
            {
                Id = addedMember.Id,
                TeamId = addedMember.TeamId,
                UserId = addedMember.UserId,
                Email = addedMember.Email,
                FirstName = addedMember.FirstName,
                LastName = addedMember.LastName,
                Role = addedMember.Role,
                JoinedAt = addedMember.JoinedAt,
                IsActive = addedMember.IsActive
            };
        }
    }
}