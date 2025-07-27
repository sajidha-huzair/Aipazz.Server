using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class UpdateMatterCommandHandler : IRequestHandler<UpdateMatterCommand, Matter>
    {
        private readonly IMatterRepository _repository;
        private readonly IMatterUpdateHistoryRepository _updateHistoryRepository; // Add this

        public UpdateMatterCommandHandler(
            IMatterRepository repository,
            IMatterUpdateHistoryRepository updateHistoryRepository) // Add this parameter
        {
            _repository = repository;
            _updateHistoryRepository = updateHistoryRepository; // Add this
        }

        public async Task<Matter> Handle(UpdateMatterCommand request, CancellationToken cancellationToken)
        {
            // Fetch the matter by ID, partition key (ClientNic), and UserId
            var matter = await _repository.GetMatterById(request.Id, request.ClientNic, request.UserId);

            if (matter == null)
            {
                throw new KeyNotFoundException($"Matter with Id '{request.Id}' and ClientNic '{request.ClientNic}' not found or does not belong to the user.");
            }

            // Create update history entry
            var updateHistory = new MatterUpdateHistory
            {
                id = Guid.NewGuid().ToString(),
                MatterId = matter.id,
                UpdatedBy = request.UserId,
                UpdatedAt = DateTime.UtcNow,
                ChangeDescription = "Matter details updated",
                UserId = request.UserId,
                ClientNic = request.ClientNic
            };

            // Update allowed properties
            matter.CaseNumber = request.CaseNumber;
            matter.Date = request.Date;
            matter.Description = request.Description;
            matter.TeamMembers = request.TeamMembers;
            matter.CourtType = request.CourtType;
            matter.StatusId = request.StatusId;
            matter.UpdatedAt = DateTime.UtcNow;

            // Save both matter and update history using separate repositories
            await _repository.UpdateMatter(matter);
            await _updateHistoryRepository.AddMatterUpdateHistory(updateHistory);

            return matter;
        }
    }
}

