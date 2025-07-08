using Aipazz.Application.client.Commands;
using Aipazz.Domian.client;
using Aipazz.Application.client.Interfaces;
using MediatR;
using System;
using System.Threading;
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
                FirstName = request.FirstName,
                LastName = request.LastName,
                Type = request.Type,
                Mobile = request.Mobile,
                Landphone = request.Landphone,
                nic = request.Nic,
                email = request.Email,
                Address = request.Address,
                CaseNumber = request.CaseNumber,
                CaseName = request.CaseName,
                UserId = request.UserId
            };
            await _clientRepository.CreateAsync(client);
            return client;
        }
    }
}