using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeeting.Commands;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class AddTeamMeetingFormCommandHandler : IRequestHandler<AddTeamMeetingFormCommand, TeamMeetingForm>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public AddTeamMeetingFormCommandHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public Task<TeamMeetingForm> Handle(AddTeamMeetingFormCommand request, CancellationToken cancellationToken)
        {
            var form = new TeamMeetingForm
            {
                Id = Guid.NewGuid(),
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

            _repository.Add(form);
            return Task.FromResult(form);
        }
    }
}