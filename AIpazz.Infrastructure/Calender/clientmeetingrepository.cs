using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;

namespace AIpazz.Infrastructure.Calender;

public class clientmeetingrepository : IclientmeetingRepository
{
    private readonly List<ClientMeeting> _meetings = new List<ClientMeeting>();

    
    
    
    
    public Task AddClientMeeting(ClientMeeting meeting)
    {
        _meetings.Add(meeting); // Adds the new meeting to the list
        return Task.CompletedTask; // Operation is complete
    }

    
    

    public clientmeetingrepository()
    {
        // Dummy data
        _meetings.Add(new ClientMeeting(
            title: "Project Kickoff Meeting",
            date: DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            time: TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
            repeat: false,
            reminder: TimeSpan.FromMinutes(30),
            description: "Discuss project kickoff details with team and client.",
            meetingLink: "https://meet.google.com/kickoff123",
            location: "Meeting Room 1, Office HQ",
            teamMembers: new List<string> { "alice@example.com", "bob@example.com" },
            clientEmail: "client@example.com"
        ));
    }


    
    
    
    
   
    
   
    
    public Task<List<ClientMeeting>> GetAllClientMeetings()
    {
        var meetings = new List<ClientMeeting>
        {
            new ClientMeeting(
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

        return Task.FromResult(meetings);
    }
    public Task<ClientMeeting> GetClientMeetingByID(int id)
    {
        var meeting = _meetings.FirstOrDefault(m => m.Id.GetHashCode() == id);

        return Task.FromResult(meeting);
    }

}