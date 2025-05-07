using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetAllMattersHandler : IRequestHandler<GetAllMattersQuery, List<Matter>>
    {
        private readonly IMatterRepository _repository;

        public GetAllMattersHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Matter>> Handle(GetAllMattersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllMatters();
        }
    }
}
