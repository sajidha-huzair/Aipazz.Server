using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Billing;
using Aipazz.Application.Billing.DTOs;

namespace Aipazz.Application.Billing.TimeEntries.Commands
{
    public class CreateTimeEntryCommand : IRequest<TimeEntryDto>
    {
        public string UserId { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal RatePerHour { get; set; }
    }
}
