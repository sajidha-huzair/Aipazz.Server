using MediatR;
using Aipazz.Domian.client;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Commands
{
    public class AddClientCommand : IRequest<Client>
    {
        public string? Name { get; set; }
        public string? Nic { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

}