using MediatR;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Tasks.Commands
{
    public class AddTaskCommand : IRequest<MatterTask>
    {
        public string? MatterId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AssignedTo { get; set; }

    }
}