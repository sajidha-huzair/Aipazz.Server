using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.DTOs;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Application.Team.Queries;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetTeamMembersHandler : IRequestHandler<GetTeamMembersQuery, List<TeamMemberDto>>
    {
        private readonly ITeamRepository _repository;

        public GetTeamMembersHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TeamMemberDto>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _repository.GetTeamMembersAsync(request.TeamId, request.UserId);
            
            return members.Select(m => new TeamMemberDto
            {
                Id = m.Id,
                TeamId = m.TeamId,
                UserId = m.UserId,
                Email = m.Email,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                IsActive = m.IsActive
            }).ToList();
        }
    }
}