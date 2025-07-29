using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Billing.Interfaces;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
{
    public class CreateCourtDateFormCommandHandler : IRequestHandler<CreateCourtDateFormCommand, Domian.Calender.CourtDateForm>
    {
        private readonly ICourtDateFormRepository _repository;
        private readonly IEmailService _emailService;

        public CreateCourtDateFormCommandHandler(ICourtDateFormRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<Domian.Calender.CourtDateForm> Handle(CreateCourtDateFormCommand request, CancellationToken cancellationToken)
        {
            var newCourtDate = new Domian.Calender.CourtDateForm
            {
                id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Title = request.Title,
                CourtDate = request.CourtDate,
                Stage = request.Stage,
                Clients = request.Clients,
                CourtType = request.CourtType,
                Reminder = request.Reminder,
                Note = request.Note,
                TeamMembers = request.TeamMembers,
                ClientEmails = request.ClientEmails
            };

            // Save to repository
            _repository.AddCourtDateForm(newCourtDate);

            // Send email notifications to team members
            if (request.TeamMemberEmails != null && request.TeamMemberEmails.Any())
            {
                // this is for team members
                await _emailService.SendCourtDateEmailToMembersAsync(
                    teamMemberEmails: request.TeamMemberEmails,
                    title: request.Title!,
                    courtType: request.CourtType!,
                    stage: request.Stage!,
                    courtDate: request.CourtDate,
                    reminder: request.Reminder,
                    note: request.Note,
                    ownerEmail: request.UserId // Assuming this is the organizer/owner email
                );
            }

            return newCourtDate;
        }
    }
}
