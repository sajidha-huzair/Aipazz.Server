using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class UpdateTeamMemberHandler : IRequestHandler<UpdateTeamMemberCommand, Unit>
    {
        private readonly ITeamRepository _repository;

        public UpdateTeamMemberHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var member = new TeamMember
            {
                FirstName = request.Member.FirstName,
                LastName = request.Member.LastName,
                Role = request.Member.Role,
                IsActive = request.Member.IsActive
            };

            await _repository.UpdateTeamMemberAsync(request.TeamId, request.MemberId, member, request.UserId);
            return Unit.Value;
        }
    }
}