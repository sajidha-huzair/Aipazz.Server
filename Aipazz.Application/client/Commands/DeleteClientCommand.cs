using MediatR;

namespace Aipazz.Application.client.Commands
{
    public class DeleteClientCommand : IRequest<Unit>
    {
        public string id { get; set; }
        public string nic { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}