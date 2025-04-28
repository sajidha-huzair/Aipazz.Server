using Aipazz.Application.Billing.TimeEntries.Commands;
<<<<<<< Updated upstream
using Aipazz.Domain.Billing;
using Aipazz.Application.Billing.Interfaces;
=======
using Aipazz.Domian.Billing;
>>>>>>> Stashed changes
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Interfaces;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class UpdateTimeEntryHandler : IRequestHandler<UpdateTimeEntryCommand, TimeEntry>
    {
        private readonly ITimeEntryRepository _repository;

        public UpdateTimeEntryHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntry> Handle(UpdateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var timeEntry = await _repository.GetTimeEntryById(request.Id, request.MatterId);
            if (timeEntry == null)
            {
                throw new KeyNotFoundException($"Time entry with Id {request.Id} and MatterId {request.MatterId} not found.");
            }

            // Update properties
            timeEntry.Description = request.Description;
            timeEntry.Duration = request.Duration;
            timeEntry.Date = request.Date;
            timeEntry.RatePerHour = request.RatePerHour;

            await _repository.UpdateTimeEntry(timeEntry);
            return timeEntry;
        }
    }
}
