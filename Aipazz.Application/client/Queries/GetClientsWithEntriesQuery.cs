using Aipazz.Domian.client;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.client.Queries
{
    public class GetClientsWithEntriesQuery : IRequest<List<Client>>
    {
        public string UserId { get; }

        public GetClientsWithEntriesQuery(string userId)
        {
            UserId = userId;
        }
    }
}