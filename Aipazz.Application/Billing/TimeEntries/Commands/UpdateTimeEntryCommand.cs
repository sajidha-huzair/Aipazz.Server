using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;

namespace Aipazz.Application.Billing.TimeEntries.Commands
{
    public class UpdateTimeEntryCommand : IRequest<TimeEntryDto>
    {
        public string Id { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
        public decimal RatePerHour { get; set; }
    }
}
