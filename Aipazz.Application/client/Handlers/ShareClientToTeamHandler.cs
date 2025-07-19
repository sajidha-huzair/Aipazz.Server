using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Interfaces;
using MediatR;

namespace Aipazz.Application.client.Handlers
{
    public class ShareClientToTeamHandler : IRequestHandler<ShareClientToTeamCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public ShareClientToTeamHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(ShareClientToTeamCommand request, CancellationToken cancellationToken)
        {
            // Get client by NIC to find the exact client, then verify ownership
            var client = await _clientRepository.GetByNicAsync(request.ClientId, request.UserId);
            if (client == null)
                throw new KeyNotFoundException("Client not found or you don't have permission to share it.");

            // Update client with TeamId
            client.TeamId = request.TeamId;

            await _clientRepository.UpdateAsync(client);
            return Unit.Value;
        }
    }
}