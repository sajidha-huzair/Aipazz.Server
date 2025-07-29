using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Commands
{
    public class UpdateTeamMeetingFormCommand : IRequest<Domian.Calender.TeamMeetingForm?>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Date { get; set; }
        public string Time { get; set; } = "";
        public DateTime Reminder { get; set; }
        public string Description { get; set; } = "";
        public string VideoConferencingLink { get; set; } = "";
        public string LocationLink { get; set; } = "";
        public List<string> TeamMembers { get; set; } = new();
    }
}