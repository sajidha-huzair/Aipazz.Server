using MediatR;

namespace Aipazz.Application.client.Commands
{
    public class DeleteClientCommand : IRequest<Unit>
    {
        public string id { get; set; }
        public string Nic { get; set; }
    }
}