using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
using Aipazz.Domian.Billing;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class UpdateTimeEntryHandler : IRequestHandler<UpdateTimeEntryCommand, TimeEntryDto>
    {
        private readonly ITimeEntryRepository _repository;

        public UpdateTimeEntryHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntryDto> Handle(UpdateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetTimeEntryById(request.Id, request.MatterId, request.UserId);
            if (existing == null) return null;

            existing.Description = request.Description;
            existing.Duration = request.Duration;
            existing.Date = request.Date;
            existing.RatePerHour = request.RatePerHour;

            await _repository.UpdateTimeEntry(existing);

            return new TimeEntryDto
            {
                Id = existing.id,
                UserId = existing.UserId,
                Description = existing.Description,
                Duration = existing.Duration,
                Date = existing.Date,
                RatePerHour = existing.RatePerHour,
                Amount = existing.Amount
            };
        }
    }
}
