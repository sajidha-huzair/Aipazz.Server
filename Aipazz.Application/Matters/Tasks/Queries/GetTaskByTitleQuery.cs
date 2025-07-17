using MediatR;
using Aipazz.Domian.Matters;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Tasks.Queries
{
    public class GetTaskByTitleQuery : IRequest<MatterTask?>
    {
        public string? Title { get; set; }
    }
}