using Aipazz.Application.client.Commands;
using Aipazz.Domian.client;
using Aipazz.Application.client.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, Client>
    {
        private readonly IClientRepository _clientRepository;

        public AddClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Client
            {
                id = Guid.NewGuid().ToString(),
                name = request.Name,
                nic = request.Nic,
<<<<<<< Updated upstream:Aipazz.Application/client/Handlers/AddClientHandler.cs
                phone = request.Phone,
                email = request.Email
=======
                email = request.Email,
                Address = request.Address,
                CaseNumber = request.CaseNumber,
                CaseName = request.CaseName,
                UserId = request.UserId
>>>>>>> Stashed changes:Aipazz.Application/client/Handlers/AddClientCommandHandler.cs
            };
            await _clientRepository.CreateAsync(client);
            return client;
        }
    }
}
