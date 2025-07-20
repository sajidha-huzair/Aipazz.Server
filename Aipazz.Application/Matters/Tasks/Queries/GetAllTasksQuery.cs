using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Tasks.Queries
{
    public record GetAllTasksQuery() : IRequest<List<MatterTask>>;
}