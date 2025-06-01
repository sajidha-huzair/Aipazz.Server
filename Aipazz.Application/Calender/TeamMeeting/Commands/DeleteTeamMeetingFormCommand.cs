using MediatR;

namespace Aipazz.Application.Calender.TeamMeetingForm.Commands
{
    public class DeleteTeamMeetingFormCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteTeamMeetingFormCommand(Guid id)
        {
            Id = id;
        }
    }
}