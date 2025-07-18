using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Application.Team.Queries;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsQuery, List<Aipazz.Domian.Team.Team>>
    {
        private readonly ITeamRepository _repository;

        public GetAllTeamsHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Aipazz.Domian.Team.Team>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            // Use both userId and userEmail to find teams
            return await _repository.GetTeamsByUserEmailAsync(request.UserEmail, request.UserId);
        }
    }
}