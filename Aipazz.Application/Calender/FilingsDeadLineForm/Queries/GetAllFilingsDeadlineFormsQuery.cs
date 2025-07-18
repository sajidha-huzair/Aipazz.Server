using Aipazz.Domain.Calender;
using Aipazz.Application.Calender.Interfaces;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.FilingsDeadlineForm.Queries
{
    public class GetAllFilingsDeadlineFormsQuery : IRequest<List<FilingsDeadlineFormEntity>> { }

    public class GetAllFilingsDeadlineFormsHandler : IRequestHandler<GetAllFilingsDeadlineFormsQuery, List<FilingsDeadlineFormEntity>>
    {
        private readonly IFilingsDeadlineFormRepository _repository;

        public GetAllFilingsDeadlineFormsHandler(IFilingsDeadlineFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FilingsDeadlineFormEntity>> Handle(GetAllFilingsDeadlineFormsQuery request, CancellationToken cancellationToken)
        {
            var forms = await _repository.GetAll();
            return forms;
        }
    }
}