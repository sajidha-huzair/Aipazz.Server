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
            var status = new Status
            {
                id = request.Id,
                Name = request.Name
            };

            await _repository.UpdateStatus(status);
            return status;
        }
    }
}
