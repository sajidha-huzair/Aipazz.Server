using MediatR;
using Aipazz.Domian.client;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Queries
{
    public class GetClientByNameQuery : IRequest<Client?>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}