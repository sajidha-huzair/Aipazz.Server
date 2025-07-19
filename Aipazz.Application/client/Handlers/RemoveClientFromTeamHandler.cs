using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Interfaces;
using MediatR;

namespace Aipazz.Application.client.Handlers
{
    public class RemoveClientFromTeamHandler : IRequestHandler<RemoveClientFromTeamCommand, bool>
    {
        private readonly IClientRepository _clientRepository;

        public RemoveClientFromTeamHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> Handle(RemoveClientFromTeamCommand request, CancellationToken cancellationToken)
        {
            // First, get client by NIC to find the exact client, then verify ownership
            var client = await _clientRepository.GetByNicAsync(request.ClientId, request.UserId);
            
            if (client == null)
                return false;

            // Check if client is actually shared with a team
            if (string.IsNullOrEmpty(client.TeamId))
                return false; // Client is not shared with any team

            // Remove team assignment
            client.TeamId = null;

            await _clientRepository.UpdateAsync(client);
            return true;
        }
    }
}