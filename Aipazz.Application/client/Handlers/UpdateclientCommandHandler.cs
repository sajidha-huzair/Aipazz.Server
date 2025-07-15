using Aipazz.Application.client.Interfaces;
using Aipazz.Domian.client;
using Aipazz.Application.client.Commands;
using MediatR;
using System.Threading;
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
            await _clientRepository.UpdateAsync(client);
            return client;
        }
    }
}