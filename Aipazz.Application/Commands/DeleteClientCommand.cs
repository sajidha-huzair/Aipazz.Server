using MediatR;

namespace Aipazz.Application.Commands
{
    public class DeleteClientCommand : IRequest
    {
        public string Id { get; set; }
    }
}