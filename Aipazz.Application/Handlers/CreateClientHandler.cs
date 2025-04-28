using Aipazz.Application.Commands;
using Aipazz.Application.Interfaces;
using Aipazz.Domian.Entities;
using MediatR;

namespace Aipazz.Application.Handlers
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, string>
    {
        private readonly IClientRepository _clientRepository;

        public CreateClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<string> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.NIC))
                throw new ArgumentException("NIC is mandatory.");

            var client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                MobileNumber = request.MobileNumber,
                CaseNumber = request.CaseNumber,
                CaseName = request.CaseName,
                NIC = request.NIC,
                CaseType = request.CaseType
            };

            await _clientRepository.AddClientAsync(client);
            return client.Id;
        }
    }
}