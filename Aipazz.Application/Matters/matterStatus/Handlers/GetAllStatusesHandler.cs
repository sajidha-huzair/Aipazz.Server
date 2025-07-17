using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterStatus.Queries;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class GetAllStatusesHandler : IRequestHandler<GetAllStatusesQuery, List<Status>>
    {
        private readonly IStatusRepository _repository;

        public GetAllStatusesHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Status>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllStatuses(request.UserId);
        }
    }
}
