using Newtonsoft.Json;

namespace Aipazz.Domian.Calender
{
    public class FilingsDeadlineForm
    {
        [JsonProperty("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();
        
        public Guid Id 
        {
            get => Guid.Parse(id);
            set => id = value.ToString();
        }
        
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Reminder { get; set; } = string.Empty; // Use as dropdown values
        public string Description { get; set; } = string.Empty;
        public string AssignedMatter { get; set; } = string.Empty; // Use as dropdown values
        
        // Partition key for Cosmos DB
        public string PartitionKey => AssignedMatter;
    }
}
