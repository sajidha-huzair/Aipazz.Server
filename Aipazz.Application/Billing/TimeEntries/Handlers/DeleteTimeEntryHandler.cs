using Aipazz.Application.Billing.TimeEntries.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Interfaces;

namespace Aipazz.Application.Billing.TimeEntries.Handlers
{
    public class DeleteTimeEntryHandler : IRequestHandler<DeleteTimeEntryCommand, bool>
    {
        private readonly ITimeEntryRepository _repository;

        public DeleteTimeEntryHandler(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var timeEntry = await _repository.GetTimeEntryById(request.Id, request.MatterId);
            if (timeEntry == null)
            {
                throw new KeyNotFoundException($"Time entry with Id {request.Id} and MatterId {request.MatterId} not found.");
            }

            await _repository.DeleteTimeEntry(request.Id, request.MatterId);
            return true;
        }
    }
}
