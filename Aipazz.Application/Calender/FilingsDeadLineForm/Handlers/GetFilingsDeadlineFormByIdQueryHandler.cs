using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.Handlers.FilingsDeadlineForms
{
    public class GetFilingsDeadlineFormByIdQueryHandler : IRequestHandler<GetFilingsDeadlineFormByIdQuery, Domian.Calender.FilingsDeadlineForm?>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public GetFilingsDeadlineFormByIdQueryHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.FilingsDeadlineForm?> Handle(GetFilingsDeadlineFormByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetById(request.Id);
        }
    }
}