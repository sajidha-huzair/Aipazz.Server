using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Queries
{
    public record GetStatusByIdQuery(string Id, string name) : IRequest<Status>;
}
