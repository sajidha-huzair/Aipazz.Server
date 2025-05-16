using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetAllMatterTitlesQuery : IRequest<List<MatterLookupDto>> { }

    public class MatterLookupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }

}
