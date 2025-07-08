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
<<<<<<< Updated upstream
=======
        public string? Address { get; set; }
        public string? CaseNumber { get; set; }
        public string? CaseName { get; set; }
        public string UserId { get; set; } = string.Empty;
>>>>>>> Stashed changes
    }

}