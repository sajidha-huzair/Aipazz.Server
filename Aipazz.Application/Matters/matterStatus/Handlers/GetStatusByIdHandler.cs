using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.matterStatus.Queries;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class GetStatusByIdHandler : IRequestHandler<GetStatusByIdQuery, Status>
    {
        private readonly IStatusRepository _repository;

        public GetStatusByIdHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<Status> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var status = await _repository.GetStatusById(request.Id, request.name); // Updated

            if (status == null)
            {
                throw new KeyNotFoundException($"Status with ID {request.Id} not found.");
            }

            return status;
        }
    }
}
