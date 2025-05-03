using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;

namespace AIpazz.Infrastructure.Calender
{
    public class clientmeetingrepository : IclientmeetingRepository
    {
        private static readonly List<ClientMeeting> _meetings = new()
        {
            new ClientMeeting(
                id: Guid.NewGuid(),
                title: "Project Kickoff Meeting",
                date: DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // Tomorrow
                time: TimeOnly.FromDateTime(DateTime.Now.AddHours(2)), // 2 hours from now
                repeat: false,
                reminder: TimeSpan.FromMinutes(30), // 30 min reminder
                description: "Discuss project kickoff details with team and client.",
                meetingLink: "https://meet.google.com/kickoff123",
                location: "Meeting Room 1, Office HQ",
                teamMembers: new List<string> { "alice@example.com", "bob@example.com" },
                clientEmail: "client@example.com"
            ),
            new ClientMeeting(
                id: Guid.NewGuid(),
                title: "Weekly Status Update",
                date: DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                time: new TimeOnly(10, 0), // 10:00 AM
                repeat: true, // Weekly
                reminder: TimeSpan.FromMinutes(15),
                description: "Weekly project status updates.",
                meetingLink: "https://meet.google.com/statusupdate",
                location: "Virtual - Google Meet",
                teamMembers: new List<string> { "teamlead@example.com", "qa@example.com" },
                clientEmail: "statusclient@example.com"
            )
        };

        public Task<ClientMeeting> GetClientMeetingByID(Guid id)
        {
            var meeting = _meetings.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(meeting);
        }

        public Task AddClientMeeting(ClientMeeting meeting)
        {
            _meetings.Add(meeting);
            return Task.CompletedTask;
        }

        public Task<List<ClientMeeting>> GetAllClientMeetings()
        {
            return Task.FromResult(_meetings);
        }
        
        
        public Task<ClientMeeting> UpdateClientMeeting(ClientMeeting meeting)
        {
            var index = _meetings.FindIndex(m => m.Id == meeting.Id);
            if (index >= 0)
            {
                _meetings[index] = meeting;
            }

            return Task.FromResult(meeting);
        }
        
        
        public Task<bool> DeleteClientMeeting(Guid id)
        {
            var meeting = _meetings.FirstOrDefault(m => m.Id == id);
            if (meeting != null)
            {
                _meetings.Remove(meeting);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        
    }
}




