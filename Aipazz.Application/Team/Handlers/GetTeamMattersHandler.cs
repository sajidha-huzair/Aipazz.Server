using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Team.Queries;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class GetTeamMattersHandler : IRequestHandler<GetTeamMattersQuery, List<Matter>>
    {
        private readonly IMatterRepository _matterRepository;

        public GetTeamMattersHandler(IMatterRepository matterRepository)
        {
            _matterRepository = matterRepository;
        }

        public async Task<List<Matter>> Handle(GetTeamMattersQuery request, CancellationToken cancellationToken)
        {
            return await _matterRepository.GetMattersByTeamIdAsync(request.TeamId);
        }
    }
}