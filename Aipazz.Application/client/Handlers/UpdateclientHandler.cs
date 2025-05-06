using Aipazz.Application.client.Interfaces;
using Aipazz.Domian.client;
using Aipazz.Application.client.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
        public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Client>
        {
            private readonly IClientRepository _clientRepository;

            public UpdateClientCommandHandler(IClientRepository clientRepository)
            {
                _clientRepository = clientRepository;
            }

            public async Task<Client> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
            {
                var client = new Client
                {
                    id = request.id,
                    name = request.Name,
                    nic = request.Nic,
                    phone = request.Phone,
                    email = request.Email
                };
                await _clientRepository.UpdateAsync(client);
                return client;
            }
        }
    }
