using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.Interface;
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
            var existing = await _repository.UpdateCourtDateForm(request.Id, param);
            if (existing == null) return null;

            return existing;
        }
    }
}