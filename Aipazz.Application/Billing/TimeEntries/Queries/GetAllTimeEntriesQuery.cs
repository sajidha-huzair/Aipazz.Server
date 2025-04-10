using Aipazz.Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.Billing.TimeEntries.Queries
{
  
        public record GetAllTimeEntriesQuery() : IRequest<List<TimeEntry>>;
    
}
