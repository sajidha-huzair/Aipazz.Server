using MediatR;

namespace Aipazz.Application.Matters.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public string? Id { get; set; } = default;
        public string? MatterId { get; set; } = default;
    }
}