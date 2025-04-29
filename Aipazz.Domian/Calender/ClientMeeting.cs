namespace Aipazz.Domian.Calender;

public class ClientMeeting
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Time { get; private set; }
    public bool Repeat { get; private set; }
    public TimeSpan? Reminder { get; private set; } // Nullable in case no reminder is set
    public string? Description { get; private set; }
    public string? MeetingLink { get; private set; }
    public string? Location { get; private set; }
    public List<string> TeamMembers { get; private set; } = new();
    public string ClientEmail { get; private set; }

    private ClientMeeting() { } // For EF Core or serialization

    public ClientMeeting(
        Guid id,
        string title,
        DateOnly date,
        TimeOnly time,
        bool repeat,
        TimeSpan? reminder,
        string? description,
        string? meetingLink,
        string? location,
        List<string> teamMembers,
        string clientEmail)
    {
        Id = id;
        Title = title;
        Date = date;
        Time = time;
        Repeat = repeat;
        Reminder = reminder;
        Description = description;
        MeetingLink = meetingLink;
        Location = location;
        TeamMembers = teamMembers ?? new List<string>();
        ClientEmail = clientEmail;
    }
    
    
    public void UpdateDetails(
        string title,
        DateOnly date,
        TimeOnly time,
        bool repeat,
        TimeSpan? reminder,
        string? description,
        string? meetingLink,
        string? location,
        List<string> teamMembers,
        string clientEmail)
    {
        Title = title;
        Date = date;
        Time = time;
        Repeat = repeat;
        Reminder = reminder;
        Description = description;
        MeetingLink = meetingLink;
        Location = location;
        TeamMembers = teamMembers ?? new();
        ClientEmail = clientEmail;
    }

    
}