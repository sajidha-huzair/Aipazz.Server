using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using MediatR;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class RemoveMatterFromTeamHandler : IRequestHandler<RemoveMatterFromTeamCommand, bool>
    {
        private readonly IMatterRepository _repository;

        public RemoveMatterFromTeamHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveMatterFromTeamCommand request, CancellationToken cancellationToken)
        {
            var matter = await _repository.GetMatterById(request.MatterId, request.ClientNic, request.UserId);
            
            if (matter == null)
                return false;

            // Remove team assignment by setting TeamId to null
            matter.TeamId = null;

            await _repository.UpdateMatter(matter);
            return true;
        }
    }
}