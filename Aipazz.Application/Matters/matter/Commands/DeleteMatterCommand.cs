using MediatR;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class DeleteMatterCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string ClientNic { get; set; } = string.Empty; // Partition Key
        public string UserId { get; set; } = string.Empty;
    }
}
