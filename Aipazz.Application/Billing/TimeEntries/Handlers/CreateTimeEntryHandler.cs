using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class CreateTimeEntryHandler : IRequestHandler<CreateTimeEntryCommand, TimeEntryDto>
    {
        private readonly ITimeEntryRepository _repository;

        public CreateTimeEntryHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntryDto> Handle(CreateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var timeEntry = new TimeEntry
            {
                id = Guid.NewGuid().ToString(),
                matterId = request.MatterId,
                Description = request.Description,
                Duration = request.Duration,
                Date = request.Date,
                RatePerHour = request.RatePerHour,
                UserId = request.UserId
            };

            await _repository.AddTimeEntry(timeEntry);

            return new TimeEntryDto
            {
                Id = timeEntry.id,
                UserId = timeEntry.UserId,
                Description = timeEntry.Description,
                Duration = timeEntry.Duration,
                Date = timeEntry.Date,
                RatePerHour = timeEntry.RatePerHour,
                Amount = timeEntry.Amount
            };
        }
    }
}
