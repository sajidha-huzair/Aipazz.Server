using Aipazz.Application.client.Interfaces;
using Aipazz.Application.Matters.DTO;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class CreateMatterCommandHandler : IRequestHandler<CreateMatterCommand, MatterDto>
    {
        private readonly IMatterRepository _matterRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IClientRepository _clientRepository; // 👈 Needed for client name

        public CreateMatterCommandHandler(
            IMatterRepository matterRepository,
            IStatusRepository statusRepository,
            IClientRepository clientRepository)
        {
            _matterRepository = matterRepository;
            _statusRepository = statusRepository;
            _clientRepository = clientRepository;
        }

        public async Task<MatterDto> Handle(CreateMatterCommand request, CancellationToken cancellationToken)
        {
            // Get "To Do" status from DB
            var toDoStatus = await _statusRepository.GetStatusByName("To Do");
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
                StatusId = toDoStatus.Name,
                TeamMembers = request.TeamMembers,
                UserId = request.UserId,
                CourtType = request.CourtType
            };

            await _matterRepository.AddMatter(matter);

            // Get client name by NIC
            var client = await _clientRepository.GetByNicAsync(matter.ClientNic);
            var clientName = client?.name ?? "Unknown";

            // Map to DTO
            var matterDto = new MatterDto
            {
                id = matter.CaseNumber, // You want to show CaseNumber as ID
                title = matter.title,
                CaseNumber = matter.CaseNumber,
                Date = matter.Date,
                Description = matter.Description,
                ClientNic = clientName, // Use name instead of NIC
                StatusId = toDoStatus.Name,
                TeamMembers = matter.TeamMembers,
                CourtType = matter.CourtType
            };

            return matterDto;
        }
    }
}
