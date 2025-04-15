using Aipazz.Domain.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Queries
{
    public class GetTimeEntryByIdQuery : IRequest<TimeEntry>
    {
        public string Id { get; set; }
        public string MatterId { get; set; }

        public GetTimeEntryByIdQuery(string id, string matterId)
        {
            Id = id;
            MatterId = matterId;
        }
    }
}
