using Aipazz.Application.Commands;
using Aipazz.Application.Interfaces;
using MediatR;

namespace Aipazz.Application.Handlers
{
    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand>
    {
        private readonly IClientRepository _clientRepository;

        public UpdateClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByIdAsync(request.Id);
            if (client == null)
                throw new KeyNotFoundException("Client not found.");

            if (string.IsNullOrEmpty(request.NIC))
                throw new ArgumentException("NIC is mandatory.");

            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Address = request.Address;
            client.MobileNumber = request.MobileNumber;
            client.CaseNumber = request.CaseNumber;
            client.CaseName = request.CaseName;
            client.NIC = request.NIC;
            client.CaseType = request.CaseType;

            await _clientRepository.UpdateClientAsync(client);
        }
    }
}