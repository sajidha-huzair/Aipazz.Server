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
        public int MatterId { get; set; }

        public GetTimeEntryByIdQuery(string id, int matterId)
        {
            Id = id;
            MatterId = matterId;
        }
    }
}
