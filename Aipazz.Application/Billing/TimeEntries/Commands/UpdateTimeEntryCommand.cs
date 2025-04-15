using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;

namespace Aipazz.Application.Billing.TimeEntries.Commands
{
    public class UpdateTimeEntryCommand : IRequest<TimeEntry>
    {
        public string Id { get; set; }= string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
        public decimal RatePerHour { get; set; }

    }
}
