using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterStatus.Queries;
using Aipazz.Domian.Matters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
            return await _repository.GetStatusById(request.Id);
        }
    }
}
