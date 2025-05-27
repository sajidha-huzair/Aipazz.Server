using Aipazz.Application.Calendar.CourtDateForms.queries;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calendar.CourtDateForms.Queries
{
    public class GetCourtDateFormByIdQueryHandler : IRequestHandler<GetCourtDateFormByIdQuery, CourtDateForm?>
    {
        private readonly ICourtDateFormRepository _repository;

        public GetCourtDateFormByIdQueryHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<CourtDateForm?> Handle(GetCourtDateFormByIdQuery request, CancellationToken cancellationToken)
        {
            var meeting =await _repository.GetById(request.Id);
            return meeting;
        }
    }
}