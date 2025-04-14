using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domain.Billing;

namespace Aipazz.Application.Billing.TimeEntries.Commands
{
    public record CreateTimeEntryCommand(int MatterId, string Description , TimeSpan Duration, DateTime Date, decimal RatePerHour) : IRequest<TimeEntry>;
    
}
