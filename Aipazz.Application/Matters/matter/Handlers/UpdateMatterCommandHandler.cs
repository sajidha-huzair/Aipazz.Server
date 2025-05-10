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
            var matter = await _repository.GetMatterById(request.Id, request.ClientNic);
            if (matter == null)
            {
                throw new KeyNotFoundException($"Matter with Id {request.Id} and Title {request.ClientNic} not found.");
            }

            // Update properties
            matter.CaseNumber = request.CaseNumber;
            matter.Date = request.Date;
            matter.Description = request.Description;
            matter.ClientNic = request.ClientNic;
            matter.TeamMembers = request.TeamMembers;

            await _repository.UpdateMatter(matter);
            return matter;
        }
    }
}
