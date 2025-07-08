using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientByNameQueryHandler : IRequestHandler<GetClientByNameQuery, Client>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientByNameQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> Handle(GetClientByNameQuery request, CancellationToken cancellationToken)
        {
<<<<<<< Updated upstream:Aipazz.Application/client/Handlers/GetClientByNameHandler.cs
            return await _clientRepository.GetByNameAsync(request.Name ?? string.Empty);
=======
            return await _clientRepository.GetByNameAsync(
                request.FirstName ?? string.Empty,
                request.LastName ?? string.Empty,
                request.UserId
            );
>>>>>>> Stashed changes:Aipazz.Application/client/Handlers/GetClientByNameQueryHandler.cs
        }
    }
}
