using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeeting.Commands;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class UpdateTeamMeetingFormCommandHandler : IRequestHandler<UpdateTeamMeetingFormCommand, TeamMeetingForm?>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public UpdateTeamMeetingFormCommandHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public Task<TeamMeetingForm?> Handle(UpdateTeamMeetingFormCommand request, CancellationToken cancellationToken)
        {
            var updated = new TeamMeetingForm
            {
                Id = request.Id,
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

            return _repository.Update(request.Id, updated);
        }
    }
}