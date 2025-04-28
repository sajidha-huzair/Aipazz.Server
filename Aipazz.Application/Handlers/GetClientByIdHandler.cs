using Aipazz.Application.Queries;
using Aipazz.Application.Interfaces;
using Aipazz.Domian.Entities;
using MediatR;

namespace Aipazz.Application.Handlers
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, Client>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientByIdHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            return await _clientRepository.GetClientByIdAsync(request.Id);
        }
    }
}