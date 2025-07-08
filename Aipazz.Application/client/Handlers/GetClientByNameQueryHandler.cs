using Aipazz.Application.client.Interfaces;
using Aipazz.Domian.client;
using Aipazz.Application.client.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientByNameQueryHandler : IRequestHandler<GetClientByNameQuery, Client?>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientByNameQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client?> Handle(GetClientByNameQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepository.GetByNameAsync(request.FirstName ?? string.Empty, request.LastName ?? string.Empty);
        }
    }
}