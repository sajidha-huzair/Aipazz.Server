namespace Aipazz.Domian.Calender
{
    public class FilingsDeadlineForm
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Reminder { get; set; } = string.Empty; // Use as dropdown values
        public string Description { get; set; } = string.Empty;
        public string AssignedMatter { get; set; } = string.Empty; // Use as dropdown values
    }
}