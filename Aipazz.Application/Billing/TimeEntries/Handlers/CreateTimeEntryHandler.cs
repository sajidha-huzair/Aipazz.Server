using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Billing.TimeEntries.Commands;
using Aipazz.Application.Billing.Interfaces;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class CreateTimeEntryHandler : IRequestHandler<CreateTimeEntryCommand, TimeEntry>
    {
        private readonly ITimeEntryRepository _repository;

        public CreateTimeEntryHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntry> Handle(CreateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var timeEntry = new TimeEntry
            {
                id = Guid.NewGuid().ToString(),
                Duration = request.Duration,
                matterId = request.MatterId,
                Description = request.Description,
                Date = request.Date,
                RatePerHour = request.RatePerHour
            };

            await _repository.AddTimeEntry(timeEntry);
            return timeEntry;
        }
    }
}
