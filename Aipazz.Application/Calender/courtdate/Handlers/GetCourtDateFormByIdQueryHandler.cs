using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.courtdate.queries



{

    
    public class GetCourtDateFormByIdQueryHandler : IRequestHandler<GetCourtDateFormByIdQuery, CourtDateForm>
    {
        private readonly ICourtDateFormRepository _repository;
    
        public GetCourtDateFormByIdQueryHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }
    
        public async Task<CourtDateForm> Handle(GetCourtDateFormByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCourtDateFormById(request.Id);
        }
    }
    
}

