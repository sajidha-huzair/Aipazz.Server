using Aipazz.Domian.client;
using MediatR;

namespace Aipazz.Application.client.Queries
{
    public class GetClientByIdQuery : IRequest<Client>
    {
        public string Id { get; }
        public string Nic { get; }
        public string UserId { get; } // 👈 Added this

        public GetClientByIdQuery(string id, string nic, string userId)
        {
            Id = id;
            Nic = nic;
            UserId = userId;
        }
    }
}