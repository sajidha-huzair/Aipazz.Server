using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetTeamClientsQueryHandler : IRequestHandler<GetTeamClientsQuery, List<Client>>
    {
        private readonly IClientRepository _repository;

        public GetTeamClientsQueryHandler(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Client>> Handle(GetTeamClientsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetClientsByTeamIdAsync(request.TeamId);
        }
    }
}