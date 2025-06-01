using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Commands
{
    public class UpdateStatusCommand : IRequest<Status>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
