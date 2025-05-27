using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand, Status>
    {
        private readonly IStatusRepository _repository;

        public UpdateStatusHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<Status> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var existingStatus = await _repository.GetStatusById(request.Id, request.name);
            if (existingStatus == null) return null;

            existingStatus.Name = request.name;
            await _repository.UpdateStatus(existingStatus);

            return existingStatus;
        }
    }
}

