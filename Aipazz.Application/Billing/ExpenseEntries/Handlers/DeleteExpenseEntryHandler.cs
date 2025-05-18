using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Handlers
{
    public class DeleteExpenseEntryHandler : IRequestHandler<DeleteExpenseEntryCommand, bool>
    {
        private readonly IExpenseEntryRepository _repository;

        public DeleteExpenseEntryHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteExpenseEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetExpenseEntryById(request.Id, request.MatterId, request.UserId);
            if (existing == null) return false;

            await _repository.DeleteExpenseEntry(request.Id, request.MatterId, request.UserId);
            return true;
        }
    }
}
