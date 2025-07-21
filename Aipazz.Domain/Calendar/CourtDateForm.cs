using System.ComponentModel.DataAnnotations.Schema;

namespace Aipazz.Domain.Calendar
{
    public class CourtDateForm
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }
        public string CourtType { get; set; } // civil or criminal
        public string Stage { get; set; } // hearing or trial
        public List<string> Clients { get; set; }
        public DateTime CourtDate { get; set; }

        public TimeSpan Reminder { get; set; } // e.g., 2 or 7 days before CourtDate

        [NotMapped]
        public string DueStatus
        {
            get
            {
                var reminderDate = CourtDate - Reminder;
                return reminderDate.Date == DateTime.Today ? "Remind" : "Not Remind";
            }
        }

        public string Note { get; set; }
        public List<string> TeamMembers { get; set; }
        public string ClientEmail { get; set; }
    }
}