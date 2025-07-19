using Aipazz.Domain.Calender;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Commands.FilingsDeadlineForms
{
    public class UpdateFilingsDeadlineFormCommand : IRequest<FilingsDeadlineFormEntity?>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Reminder { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedMatter { get; set; } = string.Empty;
    }
}