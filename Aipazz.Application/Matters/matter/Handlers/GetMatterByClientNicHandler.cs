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
    public class GetMatterByClientNicHandler : IRequestHandler<GetMattersByClientNicQuery, List<Matter>>
    {

        private readonly IMatterRepository _matterRepository;

        public GetMatterByClientNicHandler(IMatterRepository matterRepository)
        {
            _matterRepository = matterRepository;
        }
        public Task<List<Matter>> Handle(GetMattersByClientNicQuery request, CancellationToken cancellationToken)
        {
            var response = _matterRepository.GetMattersByClientNicAsync(request.ClientNic, request.UserId);
            return response;

        }
    }
}
