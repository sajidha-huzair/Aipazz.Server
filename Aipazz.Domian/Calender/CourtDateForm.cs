using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace Aipazz.Domian.Calender
{
    public class CourtDateForm
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();
        
        [JsonIgnore]
        public Guid Id 
        {
            get => Guid.Parse(id);
            set => id = value.ToString();
        }
        
        public string CaseNumber { get; set; }
        public string CourtName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        
        // Partition key for Cosmos DB
        public string PartitionKey => CaseNumber;
    }
}
