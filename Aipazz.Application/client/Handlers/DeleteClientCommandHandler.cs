using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.id) && !string.IsNullOrEmpty(request.Nic))
            {
                await _clientRepository.DeleteAsync(request.id, request.Nic);
            }
            return Unit.Value;
        }
    }
}