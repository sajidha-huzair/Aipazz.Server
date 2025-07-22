using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterTypes.Queries;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Handlers
{
    public class GetAllMatterTypesHandler : IRequestHandler<GetAllMatterTypesQuery, List<MatterType>>
    {
        private readonly IMatterTypeRepository _repository;

        public GetAllMatterTypesHandler(IMatterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MatterType>> Handle(GetAllMatterTypesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllMatterTypes(request.UserId);
        }
    }
}
