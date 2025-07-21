using Aipazz.Application.Calender.CourtDateForms.Queries;
using Aipazz.Application.Calender.Interface;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
{
    public class GetCourtDateFormByIdQueryHandler : IRequestHandler<GetCourtDateFormByIdQuery, Domian.Calender.CourtDateForm?>
    {
        private readonly ICourtDateFormRepository _repository;

        public GetCourtDateFormByIdQueryHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domian.Calender.CourtDateForm?> Handle(GetCourtDateFormByIdQuery request, CancellationToken cancellationToken)
        {
            var meeting =await _repository.GetById(request.Id);
            return meeting;
        }
    }
}