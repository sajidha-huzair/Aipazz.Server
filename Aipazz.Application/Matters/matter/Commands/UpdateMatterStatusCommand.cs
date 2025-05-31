// File: Application/Matters/matter/Commands/UpdateMatterStatusCommand.cs
using MediatR;

namespace Aipazz.Application.Matters.matter.Commands
{
    public record UpdateMatterStatusCommand(string MatterId, string ClientNIC, string NewStatusId) : IRequest<Unit>;
}
