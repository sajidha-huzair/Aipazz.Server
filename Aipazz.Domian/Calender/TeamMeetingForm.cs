using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace Aipazz.Domian.Calender
{
    public class TeamMeetingForm
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();
        
        
        [JsonIgnore]
        public Guid Id 
        {
            get => Guid.Parse(id);
            set => id = value.ToString();
        }
        
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Repeat { get; set; } = string.Empty; // e.g., "Daily", "Weekly"
        public string Reminder { get; set; } = string.Empty; // e.g., "10 mins before"
        public string Description { get; set; } = string.Empty;
        public string VideoConferencingLink { get; set; } = string.Empty;
        public string LocationLink { get; set; } = string.Empty;
        public List<string> TeamMembers { get; set; } = new(); // e.g., ["Alice", "Bob"]
        
        
        // Partition key for Cosmos DB
        public string PartitionKey => Title;
    }
}
