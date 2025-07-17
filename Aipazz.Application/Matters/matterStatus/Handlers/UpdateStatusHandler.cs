using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

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
            // Fetch the status by ID and UserId to enforce authorization
            var status = await _repository.GetStatusById(request.Id, request.UserId);

            if (status == null)
            {
                throw new KeyNotFoundException($"Status with Id '{request.Id}' not found or does not belong to the user.");
            }

            // Update allowed properties
            status.id = request.Id;
            status.Name = request.Name;

            // Persist the changes
            await _repository.UpdateStatus(status);

            return status;
        }
    }
}
