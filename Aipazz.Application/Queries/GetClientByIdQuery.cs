using Aipazz.Domian.Entities;
using MediatR;

namespace Aipazz.Application.Queries
{
    public class GetClientByIdQuery : IRequest<Client>
    {
        public string Id { get; set; }
    }
}