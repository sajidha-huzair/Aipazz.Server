using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class AddFilingsDeadlineFormCommandHandler : IRequestHandler<AddFilingsDeadlineFormCommand, Domian.Calender.FilingsDeadlineForm>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public AddFilingsDeadlineFormCommandHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.FilingsDeadlineForm> Handle(AddFilingsDeadlineFormCommand request, CancellationToken cancellationToken)
        {
            var newForm = new Domian.Calender.FilingsDeadlineForm
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
                Reminder = request.Reminder,
                Description = request.Description,
                AssignedMatter = request.AssignMatter
            };

            _repository.Add(newForm);
            return Task.FromResult(newForm);
        }
    }
}