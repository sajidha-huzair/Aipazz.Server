using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
<<<<<<< Updated upstream:Aipazz.Application/client/Handlers/DeleteClientHandler.cs
            if (request.id != null)
            {
                await _clientRepository.DeleteAsync(request.id);
=======
            if (!string.IsNullOrEmpty(request.id) && !string.IsNullOrEmpty(request.nic))
            {
                await _clientRepository.DeleteAsync(request.id, request.nic,request.UserId);
>>>>>>> Stashed changes:Aipazz.Application/client/Handlers/DeleteClientCommandHandler.cs
            }
            return Unit.Value;
        }
    }
}
