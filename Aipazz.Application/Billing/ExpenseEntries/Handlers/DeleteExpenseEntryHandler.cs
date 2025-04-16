using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
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
            var expenseEntry = await _repository.GetExpenseEntryById(request.Id, request.MatterId);
            if (expenseEntry == null)
            {
                throw new KeyNotFoundException($"Expense entry with Id {request.Id} and MatterId {request.MatterId} not found.");
            }

            await _repository.DeleteExpenseEntry(request.Id, request.MatterId);
            return true;
        }
    }
}
