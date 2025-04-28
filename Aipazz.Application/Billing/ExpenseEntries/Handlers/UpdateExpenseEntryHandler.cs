using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Interfaces;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aipazz.Application.Billing.ExpenseEntries.Handlers
{
    public class UpdateExpenseEntryHandler : IRequestHandler<UpdateExpenseEntryCommand, ExpenseEntry>
    {
        private readonly IExpenseEntryRepository _repository;

        public UpdateExpenseEntryHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntry> Handle(UpdateExpenseEntryCommand request, CancellationToken cancellationToken)
        {
            var ExpenseEntry = await _repository.GetExpenseEntryById(request.Id, request.MatterId);
            if (ExpenseEntry == null)
            {
                throw new KeyNotFoundException($"Expense entry with Id {request.Id} and MatterId {request.MatterId} not found.");
            }

            // Update properties
            ExpenseEntry.Category = request.Category;
            ExpenseEntry.Quantity = request.Quantity;
            ExpenseEntry.Rate = request.Rate;
            ExpenseEntry.Description = request.Description;
            ExpenseEntry.Date = request.Date;
            ExpenseEntry.Status = request.Status;

            await _repository.UpdateExpenseEntry(ExpenseEntry);
            return ExpenseEntry;
        }
    }
}
