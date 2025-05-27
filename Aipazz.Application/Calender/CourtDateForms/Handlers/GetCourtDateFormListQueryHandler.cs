using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calendar.CourtDateForms.Handlers
{
    public class GetCourtDateFormListQueryHandler : IRequestHandler<GetCourtDateFormListQuery, List<CourtDateForm>>
    {
        private readonly ICourtDateFormRepository _repository;

        public GetCourtDateFormListQueryHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CourtDateForm>> Handle(GetCourtDateFormListQuery request, CancellationToken cancellationToken)
        {
            var data = _repository.GetAll();
            return Task.FromResult(data);
        }
    }
}