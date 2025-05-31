using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class CreateMatterCommandHandler : IRequestHandler<CreateMatterCommand, Matter>
    {
        private readonly IMatterRepository _matterRepository;
        private readonly IStatusRepository _statusRepository;

        public CreateMatterCommandHandler(
            IMatterRepository matterRepository,
            IStatusRepository statusRepository)
        {
            _matterRepository = matterRepository;
            _statusRepository = statusRepository;
        }

        public async Task<Matter> Handle(CreateMatterCommand request, CancellationToken cancellationToken)
        {
            // Get "To Do" status from DB
            var toDoStatus = await _statusRepository.GetStatusByName("to-do");
            if (toDoStatus == null)
            {
                throw new Exception("Default status 'To Do' not found.");
            }

            // Create new Matter
            var matter = new Matter
            {
                id = Guid.NewGuid().ToString(),
                title = request.Title,
                CaseNumber = request.CaseNumber,
                Date = request.Date,
                Description = request.Description,
                ClientNic = request.ClientNic,
                StatusId = toDoStatus.id, // 👈 Default assignment
                TeamMembers = request.TeamMembers,
                CourtType = request.CourtType
            };

            await _matterRepository.AddMatter(matter);
            return matter;
        }
    }
}
