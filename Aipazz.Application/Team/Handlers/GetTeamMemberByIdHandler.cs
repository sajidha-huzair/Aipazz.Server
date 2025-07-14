using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.DTOs;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Application.Team.Queries;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetTeamMemberByIdHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberDto?>
    {
        private readonly ITeamRepository _repository;

        public GetTeamMemberByIdHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<TeamMemberDto?> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await _repository.GetTeamMemberByIdAsync(request.TeamId, request.MemberId, request.UserId);
            
            if (member == null) return null;

            return new TeamMemberDto
            {
                Id = member.Id,
                TeamId = member.TeamId,
                UserId = member.UserId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Role = member.Role,
                JoinedAt = member.JoinedAt,
                IsActive = member.IsActive
            };
        }
    }
}