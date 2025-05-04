using Aipazz.Application.Calender.courtdate.queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.courtdate.Handlers
{
    public class GetAllCourtDatesHandler : IRequestHandler<GetAllCourtDatesQuery, List<CourtDateForm>>
    {
        private readonly ICourtDateFormRepository _repository;

        public GetAllCourtDatesHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CourtDateForm>> Handle(GetAllCourtDatesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllCourtDates();
        }
    }
}