using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.Interfaces;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class DeleteStatusHandler : IRequestHandler<DeleteStatusCommand, bool>
    {
        private readonly IStatusRepository _repository;

        public DeleteStatusHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            var status = await _repository.GetStatusById(request.Id, request.name);
            if (status == null)
            {
                throw new KeyNotFoundException($"Status with Id {request.Id} and {request.name}not found.");
            }

            // Use status.Name as the partition key
            await _repository.DeleteStatus(request.Id, status.Name);
            return true;
        }
    }
}
