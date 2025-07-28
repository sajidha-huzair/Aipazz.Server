using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domain.Calender;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class AddFilingsDeadlineFormCommandHandler : IRequestHandler<AddFilingsDeadlineFormCommand, FilingsDeadlineFormEntity>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public AddFilingsDeadlineFormCommandHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<FilingsDeadlineFormEntity> Handle(AddFilingsDeadlineFormCommand request, CancellationToken cancellationToken)
        {
            var newForm = new FilingsDeadlineFormEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
                Reminder = request.Reminder,
                Description = request.Description,
                AssignedMatter = request.AssignedMatter
            };

            // ✅ Await the Add call to ensure the item is saved in Cosmos DB
            await _repository.Add(newForm);

            return newForm;
        }
    }
}