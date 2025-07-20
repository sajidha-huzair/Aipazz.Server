using Aipazz.Domian.client;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.client.Queries
{
    public class GetTeamClientsQuery : IRequest<List<Client>>
    {
        public string TeamId { get; set; }

        public GetTeamClientsQuery(string teamId)
        {
            TeamId = teamId;
        }
    }
}
