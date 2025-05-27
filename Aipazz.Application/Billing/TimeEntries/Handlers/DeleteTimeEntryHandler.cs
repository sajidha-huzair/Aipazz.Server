using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
            var existing = await _repository.GetTimeEntryById(request.Id, request.MatterId, request.UserId);
            if (existing == null) return false;

            await _repository.DeleteTimeEntry(request.Id, request.MatterId, request.UserId);
            return true;
        }
    }
}
