using MediatR;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class DeleteMatterCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // Partition Key
    }
}
