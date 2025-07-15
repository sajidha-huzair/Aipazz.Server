using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Application.Team.Queries;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, Aipazz.Domian.Team.Team?>
    {
        private readonly ITeamRepository _repository;

        public GetTeamByIdHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Aipazz.Domian.Team.Team?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTeamByIdAsync(request.TeamId, request.UserId);
        }
    }
}