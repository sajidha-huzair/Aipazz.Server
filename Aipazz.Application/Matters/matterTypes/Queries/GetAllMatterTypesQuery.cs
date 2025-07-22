using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Queries
{
    public class GetAllMatterTypesQuery : IRequest<List<MatterType>>
    {
        public string UserId { get; }

        public GetAllMatterTypesQuery(string userId)
        {
            UserId = userId;
        }
    }
}
