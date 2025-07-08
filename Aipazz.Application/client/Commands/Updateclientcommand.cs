using MediatR;
using Aipazz.Domian.client;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Commands
{
    public class UpdateClientCommand : IRequest<Client>
    {
        public string? id { get; set; }
<<<<<<< Updated upstream
        public string? Name { get; set; }
=======
        public string UserId { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public string? Mobile { get; set; }
        public string? Landphone { get; set; }
>>>>>>> Stashed changes
        public string? Nic { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    
}