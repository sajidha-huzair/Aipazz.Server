using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.TeamMeeting.Commands;
using MediatR;
using Aipazz.Application.Billing.Interfaces;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class AddTeamMeetingFormCommandHandler : IRequestHandler<AddTeamMeetingFormCommand, Domian.Calender.TeamMeetingForm>
    {
        private readonly ITeamMeetingFormRepository _repository;
        private readonly IEmailService _emailService;

        public AddTeamMeetingFormCommandHandler(ITeamMeetingFormRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<Domian.Calender.TeamMeetingForm> Handle(AddTeamMeetingFormCommand request, CancellationToken cancellationToken)
        {
            var form = new Domian.Calender.TeamMeetingForm
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
                Repeat = request.Repeat,
                Reminder = request.Reminder,
                Description = request.Description,
                VideoConferencingLink = request.VideoConferencingLink,
                LocationLink = request.LocationLink,
                TeamMembers = request.TeamMembers
            };

            // Save to in-memory DB
            _repository.Add(form);

            // Send emails to assigned team members
            if (request.TeamMemberEmails is not null && request.TeamMemberEmails.Count > 0)
            {
                await _emailService.SendTeamMeetingEmailToMembersAsync(
                    request.TeamMemberEmails, request.Title, request.Date,  request.Time, request.Description, request.VideoConferencingLink, request.LocationLink, request.UserId
                );
            }

            return form;
        }
    }
}
