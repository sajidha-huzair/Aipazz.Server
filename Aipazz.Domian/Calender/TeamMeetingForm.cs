namespace Aipazz.Domian.Calender
{
    public class TeamMeetingForm
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Repeat { get; set; } = string.Empty; // e.g., "Daily", "Weekly"
        public string Reminder { get; set; } = string.Empty; // e.g., "10 mins before"
        public string Description { get; set; } = string.Empty;
        public string VideoConferencingLink { get; set; } = string.Empty;
        public string LocationLink { get; set; } = string.Empty;
        public List<string> TeamMembers { get; set; } = new(); // e.g., ["Alice", "Bob"]
    }
}