using Aipazz.Application.Queries;
using Aipazz.Application.Interfaces;
using Aipazz.Domian.Entities;
using MediatR;

namespace Aipazz.Application.Handlers
{
    public class GetClientsBySearchTermHandler : IRequestHandler<GetClientsBySearchTermQuery, IEnumerable<Client>>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsBySearchTermHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Client>> Handle(GetClientsBySearchTermQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepository.SearchClientsAsync(request.SearchTerm);
        }
    }
}