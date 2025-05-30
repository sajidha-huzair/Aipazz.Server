using MediatR;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<Aipazz.Domian.Matters.MatterTask>
    {
        public string? Id { get; set; }
        public string? MatterId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string? Status { get; set; }
    }
}