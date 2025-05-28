using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.Commands.FilingsDeadlineForms
{
    public class UpdateFilingsDeadlineFormCommand : IRequest<Domian.Calender.FilingsDeadlineForm?>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Reminder { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignMatter { get; set; } = string.Empty;
    }
}