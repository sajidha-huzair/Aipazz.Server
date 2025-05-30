using MediatR;

namespace Aipazz.Application.Calender.Commands.FilingsDeadlineForms
{
    public class DeleteFilingsDeadlineFormCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteFilingsDeadlineFormCommand(Guid id)
        {
            Id = id;
        }
    }
}