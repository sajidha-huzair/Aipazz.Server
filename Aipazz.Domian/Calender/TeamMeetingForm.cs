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
        public string? UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public DateTime Reminder { get; set; }
        public string Description { get; set; } = string.Empty;
        public string VideoConferencingLink { get; set; } = string.Empty;
        public string LocationLink { get; set; } = string.Empty;
        public List<string> TeamMembers { get; set; } = new(); // e.g., ["Alice", "Bob"]
        
        public string matterId { get; set; } = string.Empty;
        
        // Partition key for Cosmos DB
        //public string PartitionKey => Title;
        public string PartitionKey => matterId;
    }
}
