using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Domian.Billing;
using Aipazz.Application.Billing.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Billing.DTOs;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class GetTimeEntryByIdHandler : IRequestHandler<GetTimeEntryByIdQuery, TimeEntryDto>
    {
        private readonly ITimeEntryRepository _repository;

        public GetTimeEntryByIdHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntryDto> Handle(GetTimeEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var entry = await _repository.GetTimeEntryById(request.Id, request.MatterId, request.UserId);
            if (entry == null) return null;

            return new TimeEntryDto
            {
                Id = entry.id,
                UserId = entry.UserId,
                Description = entry.Description,
                Duration = entry.Duration,
                Date = entry.Date,
                RatePerHour = entry.RatePerHour,
                Amount = entry.Amount,
                MatterTitle = string.Empty // optionally inject MatterRepository if needed
            };
        }
    }
}
