using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class CreateMatterHandler : IRequestHandler<CreateMatterCommand, Matter>
    {
        private readonly IMatterRepository _repository;

        public CreateMatterHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<Matter> Handle(CreateMatterCommand request, CancellationToken cancellationToken)
        {
            var matter = new Matter
            {
                id = Guid.NewGuid().ToString(),
                title = request.Title,
                CaseNumber = request.CaseNumber,
                Date = request.Date,
                Description = request.Description,
                ClientNic = request.ClientNic,
                TeamMembers = request.TeamMembers
            };

            await _repository.AddMatter(matter);
            return matter;
        }
    }
}
