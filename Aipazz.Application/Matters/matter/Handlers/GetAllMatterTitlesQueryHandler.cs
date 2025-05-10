using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matter.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetAllMatterTitlesQueryHandler : IRequestHandler<GetAllMatterTitlesQuery, List<MatterLookupDto>>
    {
        private readonly IMatterRepository _matterRepo;

        public GetAllMatterTitlesQueryHandler(IMatterRepository matterRepo)
        {
            _matterRepo = matterRepo;
        }

        public async Task<List<MatterLookupDto>> Handle(GetAllMatterTitlesQuery request, CancellationToken cancellationToken)
        {
            var matters = await _matterRepo.GetAllMatters();
            return matters.Select(m => new MatterLookupDto
            {
                Id = m.id,
                Title = m.title
            }).ToList();
        }
    }

}
