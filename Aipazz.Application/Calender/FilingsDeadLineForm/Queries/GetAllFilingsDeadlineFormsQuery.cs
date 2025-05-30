using Aipazz.Domian.Calender;
using Aipazz.Application.Calender.Interfaces;
using MediatR;

namespace Aipazz.Application.Calender.FilingsDeadlineForm.Queries
{
    public class GetAllFilingsDeadlineFormsQuery : IRequest<List<Domian.Calender.FilingsDeadlineForm>> { }

    public class GetAllFilingsDeadlineFormsHandler : IRequestHandler<GetAllFilingsDeadlineFormsQuery, List<Domian.Calender.FilingsDeadlineForm>>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public GetAllFilingsDeadlineFormsHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Domian.Calender.FilingsDeadlineForm>> Handle(GetAllFilingsDeadlineFormsQuery request, CancellationToken cancellationToken)
        {
            var forms = _repository.GetAll();
            return Task.FromResult(forms);
        }
    }
}