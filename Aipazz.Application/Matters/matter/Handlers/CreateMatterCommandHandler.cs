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
            try
            {
                Console.WriteLine($"Creating matter for user: {request.UserId}");
                Console.WriteLine($"Client NIC: {request.ClientNic}");

                // Get "To Do" status from DB
                var toDoStatus = await _statusRepository.GetStatusByName("To Do", request.UserId);

                if (toDoStatus == null)
                {
                    throw new Exception("Default status 'To Do' not found.");
                }

                // Verify client exists
                var client = await _clientRepository.GetByNicAsync(request.ClientNic,request.UserId);
                if (client == null)
                {
                    throw new Exception($"Client with NIC '{request.ClientNic}' not found.");
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
                    StatusId = string.IsNullOrEmpty(request.StatusId) ? toDoStatus.Name : request.StatusId,
                    TeamMembers = request.TeamMembers,
                    UserId = request.UserId,
                    CourtType = request.CourtType
                };

                await _matterRepository.AddMatter(matter);

                // Map to DTO - return actual database values
                var matterDto = new MatterDto
                {
                    id = matter.id, // ✅ Return actual database ID
                    title = matter.title,
                    CaseNumber = matter.CaseNumber,
                    Date = matter.Date,
                    Description = matter.Description,
                    ClientNic = matter.ClientNic, // ✅ Keep as NIC for consistency
                    StatusId = string.IsNullOrEmpty(request.StatusId) ? toDoStatus.Name : request.StatusId,
                    TeamMembers = matter.TeamMembers,
                    CourtType = matter.CourtType
                };

                Console.WriteLine($"✅ Matter created successfully: {matter.id}");
                return matterDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in CreateMatterCommandHandler: {ex.Message}");
                throw;
            }
        }
    }
}
