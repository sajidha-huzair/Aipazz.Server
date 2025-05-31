using Aipazz.Domian.client;
using MediatR;
using System;

namespace Aipazz.Application.client.Queries
{
    public class GetClientWithDetailsQuery : IRequest<Client?>
    {
        public string ClientNic { get; set; }
        public string UserId { get; set; }

        public GetClientWithDetailsQuery(string clientNic, string userId)
        {
            ClientNic = clientNic;
            UserId = userId;
        }
    }
}