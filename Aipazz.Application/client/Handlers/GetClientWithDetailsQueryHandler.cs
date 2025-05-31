using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientWithDetailsQueryHandler : IRequestHandler<GetClientWithDetailsQuery, Client?>
    {
        private readonly IClientRepository _clientRepo;

        public GetClientWithDetailsQueryHandler(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public async Task<Client?> Handle(GetClientWithDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepo.GetByNicAsync(request.ClientNic);
        }
    }
}