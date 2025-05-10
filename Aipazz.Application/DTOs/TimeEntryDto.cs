using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DTOs
{
    public class TimeEntryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Amount { get; set; }
        public string MatterTitle { get; set; } = string.Empty;

    }

}
