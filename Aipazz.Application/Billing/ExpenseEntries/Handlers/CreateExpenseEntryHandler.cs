using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.Interfaces;
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
    public class CreateExpenseEntryHandler : IRequestHandler<CreateExpenseEntryCommand, ExpenseEntry>
    {
        private readonly IExpenseEntryRepository _repository;

        public CreateExpenseEntryHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntry> Handle(CreateExpenseEntryCommand request, CancellationToken cancellationToken)
        {
            var ExpenseEntry = new ExpenseEntry
            {
                id = Guid.NewGuid().ToString(),
                matterId = request.MatterId,
                Category =request.Category,
                Quantity=request.Quantity,
                Rate=request.Rate,
                Description = request.Description,
                Date = request.Date,
                Status=request.Status
            };

            await _repository.AddExpenseEntry(ExpenseEntry);
            return ExpenseEntry;
        }
    }
}