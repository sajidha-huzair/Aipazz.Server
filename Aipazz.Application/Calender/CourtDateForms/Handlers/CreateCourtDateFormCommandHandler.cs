using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.Interface;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
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
                Title = request.Title,
                CourtDate = request.CourtDate,
                Stage = request.Stage,
                Clients = request.Clients,
                CourtType = request.CourtType,
                Reminder = request.Reminder,
                Note = request.Note,
                TeamMembers = request.TeamMembers,
                ClientEmail = request.ClientEmail
            };

            _repository.AddCourtDateForm(newCourtDate);
            return Task.FromResult(newCourtDate);
        }
    }
}