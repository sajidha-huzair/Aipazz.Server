using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class UpdateFilingsDeadlineFormCommandHandler : IRequestHandler<UpdateFilingsDeadlineFormCommand, Domian.Calender.FilingsDeadlineForm?>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public UpdateFilingsDeadlineFormCommandHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.FilingsDeadlineForm?> Handle(UpdateFilingsDeadlineFormCommand request, CancellationToken cancellationToken)
        {
            var updatedForm = new Domian.Calender.FilingsDeadlineForm
            {
                Id = request.Id,
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
                Reminder = request.Reminder,
                Description = request.Description,
                AssignedMatter = request.AssignMatter
            };

            return _repository.Update(request.Id, updatedForm);
        }
    }
}