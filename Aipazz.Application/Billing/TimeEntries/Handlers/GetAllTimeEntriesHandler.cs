using Aipazz.Domian.Billing;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.DTOs;
using Aipazz.Application.Matters.Interfaces;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class GetAllTimeEntriesHandler : IRequestHandler<GetAllTimeEntriesQuery, List<TimeEntryDto>>
    {
        private readonly ITimeEntryRepository _repository;
        private readonly IMatterRepository _matterRepository;

        public GetAllTimeEntriesHandler(ITimeEntryRepository repository, IMatterRepository matterRepository)
        {
            _repository = repository;
            _matterRepository = matterRepository;
        }

        public async Task<List<TimeEntryDto>> Handle(GetAllTimeEntriesQuery request, CancellationToken cancellationToken)
        {
            var entries = await _repository.GetAllTimeEntries();
            var matters = await _matterRepository.GetAllMatters();

            return entries.Select(e => new TimeEntryDto
            {
                Id = e.id,
                MatterTitle = matters.FirstOrDefault(m => m.id == e.matterId)?.title ?? "",
                Description = e.Description,
                Date = e.Date,
                RatePerHour = e.RatePerHour,
                Amount = e.Amount,
                Duration = e.Duration

            }).ToList();
        }
    }
}
