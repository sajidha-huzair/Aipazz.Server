using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Aipazz.Domian.Calender
{
    public class CourtDateForm
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [JsonIgnore] // You can keep Guid-based Id internally if needed
        public Guid Id
        {
            get => Guid.Parse(id);
            set => id = value.ToString();
        }

        public string UserId { get; set; }

        public string? Title { get; set; }
        public string? CourtType { get; set; }
        public string? Stage { get; set; }
        public List<string>? Clients { get; set; }
        public DateTime CourtDate { get; set; }

        public DateTime Reminder { get; set; }

        [NotMapped]
        public string DueStatus
        {
            get
            {
                return Reminder.Date == DateTime.Today ? "Remind" : "Not Remind";
            }
        }

        public string? Note { get; set; }
        public List<string>? TeamMembers { get; set; }
        public List<string> ClientEmails { get; set; }

        public string PartitionKey => id;
    }
}