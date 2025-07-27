using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Queries
{
    public class GetMatterTypeByIdQuery : IRequest<MatterType?>
    {
        public string Id { get; }
        public string UserId { get; }

        public GetMatterTypeByIdQuery(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
