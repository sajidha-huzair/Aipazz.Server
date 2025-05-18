using System.Collections.Generic;
using MediatR;
using Aipazz.Application.Billing.DTOs;

namespace Aipazz.Application.Billing.TimeEntries.Queries
    {
    public class GetAllTimeEntriesQuery : IRequest<List<TimeEntryDto>>
    {
        public string UserId { get; }

        public GetAllTimeEntriesQuery(string userId)
        {
            UserId = userId;
        }
    }

}

