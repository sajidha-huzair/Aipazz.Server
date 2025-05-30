using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.Interfaces;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
{
    public class UpdateCourtDateFormCommandHandler : IRequestHandler<UpdateCourtDateFormCommand, Domian.Calender.CourtDateForm?>
    {
        private readonly ICourtDateFormRepository _repository;

        public UpdateCourtDateFormCommandHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domian.Calender.CourtDateForm?> Handle(UpdateCourtDateFormCommand request, CancellationToken cancellationToken)
        {
            var param = new Domian.Calender.CourtDateForm
            {
                Id = request.Id,
                CaseNumber = request.CaseNumber,
                Date = request.Date,
                CourtName = request.CourtName
            };
            var existing = await _repository.UpdateCourtDateForm(request.Id, param);
            if (existing == null) return null;

            return existing;
        }
    }
}