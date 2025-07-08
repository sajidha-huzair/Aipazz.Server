using MediatR;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Commands
{
    public class DeleteClientCommand : IRequest<Unit>
    {
<<<<<<< Updated upstream
        public string? id { get; set; }
=======
        public string id { get; set; }
        public string nic { get; set; }
        public string UserId { get; set; } = string.Empty;
>>>>>>> Stashed changes
    }

   
}