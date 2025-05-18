using Aipazz.Application.Billing.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.client.Queries
{
    public class GetClientsWithEntriesQuery : IRequest<List<ClientWithMattersDto>>
    {
        public string UserId { get; }

        public GetClientsWithEntriesQuery(string userId)
        {
            UserId = userId;
        }
    }

}
