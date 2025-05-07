using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetMatterByIdQuery : IRequest<Matter>
    {
        public string Id { get; set; }
        public string Title { get; set; } // Partition Key

        public GetMatterByIdQuery(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
