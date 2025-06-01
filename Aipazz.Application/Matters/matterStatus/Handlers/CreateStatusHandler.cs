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
                id = Guid.NewGuid().ToString(),
                Name = request.Name,
                UserId = request.UserId 
            };

            await _repository.AddStatus(status);
            return status;
        }
    }
}
