using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Billing
{
    public class TimeEntry
    {
        public string id { get; set; } = string.Empty;

        // Duration in hours, minutes, and seconds
        public TimeSpan Duration { get; set; }

        // Foreign key reference to Matter table
        public string matterId { get; set; } = string.Empty;

        // Description of the work done
        public string Description { get; set; } = string.Empty;

        // Automatically set to today's date
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Rate per hour
        public decimal RatePerHour { get; set; }

        // Auto-calculated amount (Duration in hours * RatePerHour)
        public decimal Amount => (decimal)Duration.TotalHours * RatePerHour;
    }
}
