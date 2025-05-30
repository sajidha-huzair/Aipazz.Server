using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Commands
{
    public class AddTeamMeetingFormCommand : IRequest<TeamMeetingForm>
    {
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string Repeat { get; set; } = null!;
        public string Reminder { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string VideoConferencingLink { get; set; } = null!;
        public string LocationLink { get; set; } = null!;
        public List<string> TeamMembers { get; set; } = new();
    }
}