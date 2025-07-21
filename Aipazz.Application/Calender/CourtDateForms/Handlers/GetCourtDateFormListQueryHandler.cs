using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.Interface;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
{
    public class GetCourtDateFormListQueryHandler : IRequestHandler<GetCourtDateFormListQuery, List<Domian.Calender.CourtDateForm>>
    {
        private readonly ICourtDateFormRepository _repository;

        public GetCourtDateFormListQueryHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Domian.Calender.CourtDateForm>> Handle(GetCourtDateFormListQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAll();
            return data;
        }
    }
}