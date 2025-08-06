using System.Text.Json.Serialization;

namespace Aipazz.Domian.Calender;

public class ClientMeeting
{
    [JsonPropertyName("id")]
    public string id { get; set; } = Guid.NewGuid().ToString();

    [JsonIgnore]
    public Guid Id 
    {
        get => Guid.Parse(id);
        set => id = value.ToString();
    }

    public  string UserId { get; set; }

    public string Title { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Time { get; private set; }
    public DateTime Reminder { get; private set; }
    public string? Description { get; private set; }
    public string? MeetingLink { get; private set; }
    public string? Location { get; private set; }
    public List<string> TeamMembers { get; private set; } = new(); // team member names
    public List<string> ClientEmails { get; private set; }
    
    public string matterId { get; set; } = string.Empty;

    //public string PartitionKey => ClientEmail;
    public string PartitionKey => matterId;
    private ClientMeeting() { }

    public ClientMeeting(
        Guid id,
        string UserId,
        string title,
        DateOnly date,
        TimeOnly time,
        DateTime reminder,
        string? description,
        string? meetingLink,
        string? location,
        List<string> teamMembers,
        List<string> clientEmails,
        string matterId)
    {
        Id = id;
        Title = title;
        this.UserId = UserId;
        Date = date;
        Time = time;
        Reminder = reminder;
        Description = description;
        MeetingLink = meetingLink;
        Location = location;
        TeamMembers = teamMembers ?? new List<string>();
        ClientEmails = clientEmails;
        this.matterId = matterId;
    }

    public void UpdateDetails(
        string title,
        DateOnly date,
        TimeOnly time,
        DateTime reminder,
        string? description,
        string? meetingLink,
        string? location,
        List<string> teamMembers,
        List<string> clientEmails)
    {
        Title = title;
        Date = date;
        Time = time;
        Reminder = reminder;
        Description = description;
        MeetingLink = meetingLink;
        Location = location;
        TeamMembers = teamMembers ?? new();
        ClientEmails = clientEmails;
    }
}
