using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using MediatR;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class ShareMatterToTeamHandler : IRequestHandler<ShareMatterToTeamCommand, bool>
    {
        private readonly IMatterRepository _repository;

        public ShareMatterToTeamHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ShareMatterToTeamCommand request, CancellationToken cancellationToken)
        {
            var matter = await _repository.GetMatterById(request.MatterId, request.ClientNic, request.UserId);
            
            if (matter == null)
                return false;

            // Update matter with team ID
            matter.TeamId = request.TeamId;

            await _repository.UpdateMatter(matter);
            return true;
        }
    }
}