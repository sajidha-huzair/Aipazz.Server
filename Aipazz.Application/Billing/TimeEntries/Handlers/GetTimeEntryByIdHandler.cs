using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Domian.Billing;
using Aipazz.Application.Billing.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class GetTimeEntryByIdHandler : IRequestHandler<GetTimeEntryByIdQuery, TimeEntry>
    {
        private readonly ITimeEntryRepository _repository;

        public GetTimeEntryByIdHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntry> Handle(GetTimeEntryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTimeEntryById(request.Id, request.MatterId);
        }
    }
}
