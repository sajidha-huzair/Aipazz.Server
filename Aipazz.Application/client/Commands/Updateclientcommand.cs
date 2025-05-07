using MediatR;
using Aipazz.Domian.client;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Commands
{
    public class UpdateClientCommand : IRequest<Client>
    {
        public string? id { get; set; }
        public string? Name { get; set; }
        public string? Nic { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    
}