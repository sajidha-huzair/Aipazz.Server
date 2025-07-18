using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using Aipazz.Domain.Calender;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class GetFilingsDeadlineFormByIdQueryHandler : IRequestHandler<GetFilingsDeadlineFormByIdQuery, FilingsDeadlineFormEntity?>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public GetFilingsDeadlineFormByIdQueryHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<FilingsDeadlineFormEntity?> Handle(GetFilingsDeadlineFormByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetById(request.Id);
        }
    }
}