using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetTeamSharedMattersQueryHandler : IRequestHandler<GetTeamSharedMattersQuery, List<Matter>>
    {
        private readonly IMatterRepository _repository;

        public GetTeamSharedMattersQueryHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Matter>> Handle(GetTeamSharedMattersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMattersByTeamIdAsync(request.TeamId);
        }
    }
}