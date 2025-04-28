using Aipazz.Domian.Entities;
using MediatR;

namespace Aipazz.Application.Queries
{
    public class GetClientsBySearchTermQuery : IRequest<IEnumerable<Client>>
    {
        public string SearchTerm { get; set; }
    }
}