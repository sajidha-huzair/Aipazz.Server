using MediatR;
using Aipazz.Domian.Matters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Tasks.Queries
{
    public class GetTasksByMatterIdQuery : IRequest<List<MatterTask>>
    {
        public string? MatterId { get; set; }
    }
}