using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Commands
{
    public record CreateStatusCommand(string Name) : IRequest<Status>;
}
