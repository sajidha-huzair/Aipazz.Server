using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetTeamSharedMattersQuery : IRequest<List<Matter>>
    {
        public string TeamId { get; set; }

        public GetTeamSharedMattersQuery(string teamId)
        {
            TeamId = teamId;
        }
    }
}