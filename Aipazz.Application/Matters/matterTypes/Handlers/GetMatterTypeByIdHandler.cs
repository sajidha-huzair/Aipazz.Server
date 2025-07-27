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
    public class GetMatterTypeByIdHandler : IRequestHandler<GetMatterTypeByIdQuery, MatterType?>
    {
        private readonly IMatterTypeRepository _repository;

        public GetMatterTypeByIdHandler(IMatterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<MatterType?> Handle(GetMatterTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMatterTypeById(request.Id, request.UserId);
        }
    }
}
