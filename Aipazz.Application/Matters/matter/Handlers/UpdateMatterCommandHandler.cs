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

        public UpdateMatterCommandHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<Matter> Handle(UpdateMatterCommand request, CancellationToken cancellationToken)
        {
            // Fetch the matter by ID, partition key (ClientNic), and UserId
            var matter = await _repository.GetMatterById(request.Id, request.ClientNic, request.UserId);

            if (matter == null)
            {
                throw new KeyNotFoundException($"Matter with Id '{request.Id}' and ClientNic '{request.ClientNic}' not found or does not belong to the user.");
            }

            // Update allowed properties
            matter.CaseNumber = request.CaseNumber;
            matter.Date = request.Date;
            matter.Description = request.Description;
            matter.ClientNic = request.ClientNic;
            matter.TeamMembers = request.TeamMembers;
            matter.CourtType = request.CourtType;
            matter.StatusId = request.StatusId;

            // Persist the changes
            await _repository.UpdateMatter(matter);

            return matter;
        }
    }
}
