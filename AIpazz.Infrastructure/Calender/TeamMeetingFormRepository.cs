using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;

namespace AIpazz.Infrastructure.Calendar
{
    public class TeamMeetingFormRepository : ITeamMeetingFormRepository
    {
        private readonly List<TeamMeetingForm> _forms = new();

        public TeamMeetingFormRepository()
        {
            _forms.Add(new TeamMeetingForm
            {
                Id = Guid.NewGuid(),
                Title = "Project Kickoff",
                Date = DateTime.UtcNow.AddDays(1),
                Time = "3:00 PM",
                Repeat = "Weekly",
                Reminder = "30 minutes before",
                Description = "Initial project planning meeting.",
                VideoConferencingLink = "https://meet.example.com/abc123",
                LocationLink = "https://maps.google.com/example-location",
                TeamMembers = new List<string> { "John", "Jane", "Alice" }
            });
        }

        public List<TeamMeetingForm> GetAll()
        {
            return _forms;
        }

        public Task<TeamMeetingForm?> GetById(Guid id)
        {
            var form = _forms.FirstOrDefault(f => f.Id == id);
            return Task.FromResult(form);
        }

        public void Add(TeamMeetingForm form)
        {
            _forms.Add(form);
        }

        public Task<TeamMeetingForm?> Update(Guid id, TeamMeetingForm updatedForm)
        {
            var existing = _forms.FirstOrDefault(f => f.Id == id);
            if (existing == null) return Task.FromResult<TeamMeetingForm?>(null);

            existing.Title = updatedForm.Title;
            existing.Date = updatedForm.Date;
            existing.Time = updatedForm.Time;
            existing.Repeat = updatedForm.Repeat;
            existing.Reminder = updatedForm.Reminder;
            existing.Description = updatedForm.Description;
            existing.VideoConferencingLink = updatedForm.VideoConferencingLink;
            existing.LocationLink = updatedForm.LocationLink;
            existing.TeamMembers = updatedForm.TeamMembers;

            return Task.FromResult(existing);
        }

        public Task<bool> Delete(Guid id)
        {
            var form = _forms.FirstOrDefault(f => f.Id == id);
            if (form == null) return Task.FromResult(false);

            _forms.Remove(form);
            return Task.FromResult(true);
        }
    }
}
