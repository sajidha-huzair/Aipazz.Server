using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.TeamMeeting.Commands;

using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class UpdateTeamMeetingFormCommandHandler : IRequestHandler<UpdateTeamMeetingFormCommand, Domian.Calender.TeamMeetingForm?>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public UpdateTeamMeetingFormCommandHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.TeamMeetingForm?> Handle(UpdateTeamMeetingFormCommand request, CancellationToken cancellationToken)
        {
            var updated = new Domian.Calender.TeamMeetingForm
            {
                Id = request.Id,
                Title = request.Title,
                Date = request.Date,
                Time = request.Time,
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