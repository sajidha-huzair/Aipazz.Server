using MediatR;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Commands
{
    public class DeleteClientCommand : IRequest<Unit>
    {
        public string? id { get; set; }
    }

   
}