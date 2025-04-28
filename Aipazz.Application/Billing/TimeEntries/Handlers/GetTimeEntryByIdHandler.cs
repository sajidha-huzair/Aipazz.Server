using Aipazz.Application.Billing.TimeEntries.Queries;
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
