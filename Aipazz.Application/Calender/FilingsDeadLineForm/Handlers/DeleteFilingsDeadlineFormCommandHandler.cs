using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.Interfaces;
using MediatR;

namespace Aipazz.Application.Calender.FilingsDeadlineForm.Handlers
{
    public class DeleteFilingsDeadlineFormCommandHandler : IRequestHandler<DeleteFilingsDeadlineFormCommand, bool>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public DeleteFilingsDeadlineFormCommandHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteFilingsDeadlineFormCommand request, CancellationToken cancellationToken)
        {
            return await _repository.Delete(request.Id);
        }

    }
}