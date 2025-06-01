using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetAllMatterTitlesQuery : IRequest<List<MatterLookupDto>>
    {
        public string UserId { get; set; }

        public GetAllMatterTitlesQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class MatterLookupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
