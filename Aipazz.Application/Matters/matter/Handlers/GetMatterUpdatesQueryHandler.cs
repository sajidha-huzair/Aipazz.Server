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
    public class GetMatterUpdatesQueryHandler : IRequestHandler<GetMatterUpdatesQuery, List<MatterUpdateHistory>>
    {
        private readonly IMatterUpdateHistoryRepository _repository; // Change this

        public GetMatterUpdatesQueryHandler(IMatterUpdateHistoryRepository repository) // Change this
        {
            _repository = repository;
        }

        public async Task<List<MatterUpdateHistory>> Handle(GetMatterUpdatesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMatterUpdateHistory(request.MatterId, request.ClientNic, request.UserId);
        }
    }
}
