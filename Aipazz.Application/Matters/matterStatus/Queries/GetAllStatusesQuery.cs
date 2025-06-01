using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;


namespace Aipazz.Application.Matters.matterStatus.Queries
{
    public class GetAllStatusesQuery : IRequest<List<Status>>
    {
        public string UserId { get; set; }

        public GetAllStatusesQuery(string userId)
        {
            UserId = userId;
        }
    }

}
