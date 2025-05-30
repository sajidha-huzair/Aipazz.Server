using Aipazz.Application.Calender.CourtDateForm.Commands;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForm.Handlers
{
    public class CreateCourtDateFormCommandHandler : IRequestHandler<CreateCourtDateFormCommand, Domian.Calender.CourtDateForm>
    {
        private readonly ICourtDateFormRepository _repository;

        public CreateCourtDateFormCommandHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.CourtDateForm> Handle(CreateCourtDateFormCommand request, CancellationToken cancellationToken)
        {
            var newCourtDate = new Domian.Calender.CourtDateForm
            {
                Id = Guid.NewGuid(),
                CaseNumber = request.CaseNumber,
                CourtName = request.CourtName,
                Date = request.Date
            };

            _repository.AddCourtDateForm(newCourtDate);
            return Task.FromResult(newCourtDate);
        }
    }
}