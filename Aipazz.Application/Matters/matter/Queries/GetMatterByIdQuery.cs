using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetMatterByIdQuery : IRequest<Matter>
    {
        public string Id { get; set; }
        public string clientNic { get; set; } // Partition Key

        public GetMatterByIdQuery(string id, string ClientNic)
        {
            Id = id;
            clientNic = ClientNic;
        }
    }
}
