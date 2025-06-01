// File: Application/Matters/matter/Handlers/GetMattersByStatusIdHandler.cs
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetMattersByStatusIdHandler : IRequestHandler<GetMattersByStatusIdQuery, List<Matter>>
    {
        private readonly IMatterRepository _repository;

        public GetMattersByStatusIdHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Matter>> Handle(GetMattersByStatusIdQuery request, CancellationToken cancellationToken)
        {
            var allMatters = await _repository.GetAllMatters(request.UserId);
            return allMatters.Where(m => m.StatusId == request.StatusId).ToList();
        }
    }
}

