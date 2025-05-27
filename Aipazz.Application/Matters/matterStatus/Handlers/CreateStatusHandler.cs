using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class CreateStatusHandler : IRequestHandler<CreateStatusCommand, Status>
    {
        private readonly IStatusRepository _repository;

        public CreateStatusHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<Status> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = new Status
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.name
            };

            await _repository.AddStatus(status);
            return status;
        }
    }
}

