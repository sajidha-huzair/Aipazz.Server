using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetMatterByIdQuery : IRequest<Matter>
    {
        public string Id { get; set; }
        public string ClientNic { get; set; }
        public string UserId { get; set; } // 👈 Add this

        public GetMatterByIdQuery(string id, string clientNic, string userId)
        {
            Id = id;
            ClientNic = clientNic;
            UserId = userId;
        }
    }
}
