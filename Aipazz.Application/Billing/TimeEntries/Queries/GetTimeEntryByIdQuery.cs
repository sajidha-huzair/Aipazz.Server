using Aipazz.Application.Billing.DTOs;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Queries
{
    public class GetTimeEntryByIdQuery(string id, string matterId, string userId) : IRequest<TimeEntryDto>
    {
        public string Id { get; } = id;
        public string MatterId { get; } = matterId;
        public string UserId { get; } = userId;
    }
}

