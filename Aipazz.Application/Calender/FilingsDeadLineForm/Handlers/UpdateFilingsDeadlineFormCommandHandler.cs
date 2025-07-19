using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domain.Calender;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class UpdateFilingsDeadlineFormCommandHandler : IRequestHandler<UpdateFilingsDeadlineFormCommand, FilingsDeadlineFormEntity?>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public UpdateFilingsDeadlineFormCommandHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<FilingsDeadlineFormEntity?> Handle(UpdateFilingsDeadlineFormCommand request, CancellationToken cancellationToken)
        {
            var updatedForm = new FilingsDeadlineFormEntity
            {
                Id = request.Id,
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
                Reminder = request.Reminder,
                Description = request.Description,
                AssignedMatter = request.AssignedMatter
            };

            return await _repository.Update(request.Id, updatedForm);
        }
    }
}