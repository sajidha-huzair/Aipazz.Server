using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.DTOs;
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
    public class CreateExpenseEntryHandler : IRequestHandler<CreateExpenseEntryCommand, ExpenseEntryDto>
    {
        private readonly IExpenseEntryRepository _repository;

        public CreateExpenseEntryHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntryDto> Handle(CreateExpenseEntryCommand request, CancellationToken cancellationToken)
        {
            var expenseEntry = new ExpenseEntry
            {
                id = Guid.NewGuid().ToString(),
                matterId = request.MatterId,
                Category =request.Category,
                Quantity=request.Quantity,
                Rate=request.Rate,
                Description = request.Description,
                Date = request.Date,
                UserId=request.UserId
            };

            await _repository.AddExpenseEntry(expenseEntry);
            return new ExpenseEntryDto
            {
                Id = expenseEntry.id,
                UserId = expenseEntry.UserId,
                Category = expenseEntry.Category,
                Quantity = expenseEntry.Quantity,
                Description = expenseEntry.Description,
                Date = expenseEntry.Date,
                Rate = expenseEntry.Rate,
                Amount = expenseEntry.Amount
            };
        }
    }
}