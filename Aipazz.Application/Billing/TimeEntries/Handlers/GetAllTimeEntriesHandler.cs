using Aipazz.Domian.Billing;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Application.Interfaces;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class GetAllTimeEntriesHandler : IRequestHandler<GetAllTimeEntriesQuery, List<TimeEntry>>
    {
        private readonly ITimeEntryRepository _repository;

        public GetAllTimeEntriesHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TimeEntry>> Handle(GetAllTimeEntriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllTimeEntries();
        }
    }
}
