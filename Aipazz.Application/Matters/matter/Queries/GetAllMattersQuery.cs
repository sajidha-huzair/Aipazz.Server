using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetAllMattersQuery : IRequest<List<Matter>>
    {
        public string UserId { get; set; }

        public GetAllMattersQuery(string userId)
        {
            UserId = userId;
        }
    }
}
