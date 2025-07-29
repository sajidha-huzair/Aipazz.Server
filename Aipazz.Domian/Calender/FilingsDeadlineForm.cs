using System.Text.Json.Serialization;

namespace Aipazz.Domain.Calender
{
    public class FilingsDeadlineForm
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [JsonIgnore]  // Corrected here
        public Guid Id
        {
            get => Guid.Parse(id);
            set => id = value.ToString();
        }
        
        public string? UserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public DateTime Reminder { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AssignedMatter { get; set; } = string.Empty;
        
        public string matterId { get; set; } = string.Empty;
        //public string PartitionKey => AssignedMatter;
        public string PartitionKey => matterId;
    }
}