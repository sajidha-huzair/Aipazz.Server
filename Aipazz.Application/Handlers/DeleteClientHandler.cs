using Aipazz.Application.Commands;
using Aipazz.Application.Interfaces;
using MediatR;

namespace Aipazz.Application.Handlers
{
    public class DeleteClientHandler : IRequestHandler<DeleteClientCommand>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByIdAsync(request.Id);
            if (client == null)
                throw new KeyNotFoundException("Client not found.");

            await _clientRepository.DeleteClientAsync(request.Id);
        }
    }
}