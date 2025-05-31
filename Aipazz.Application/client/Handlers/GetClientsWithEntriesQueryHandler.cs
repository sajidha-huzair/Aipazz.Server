using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientsWithEntriesQueryHandler : IRequestHandler<GetClientsWithEntriesQuery, List<Client>>
    {
        private readonly IClientRepository _clientRepo;

        public GetClientsWithEntriesQueryHandler(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public async Task<List<Client>> Handle(GetClientsWithEntriesQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepo.GetAllClients();
        }
    }
}