using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetMattersByMatterTypeIdHandler : IRequestHandler<GetMattersByMatterTypeIdQuery, List<Matter>>
    {
        private readonly IMatterRepository _repository;

        public GetMattersByMatterTypeIdHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Matter>> Handle(GetMattersByMatterTypeIdQuery request, CancellationToken cancellationToken)
        {
            var allMatters = await _repository.GetAllMatters(request.UserId);
            return allMatters.Where(m => m.MatterTypeName == request.MatterTypeName).ToList();
        }
    }
}
