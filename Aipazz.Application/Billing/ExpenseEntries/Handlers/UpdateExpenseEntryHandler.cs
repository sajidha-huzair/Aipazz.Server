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
    public class UpdateExpenseEntryHandler : IRequestHandler<UpdateExpenseEntryCommand, ExpenseEntryDto>
    {
        private readonly IExpenseEntryRepository _repository;

        public UpdateExpenseEntryHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntryDto> Handle(UpdateExpenseEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetExpenseEntryById(request.Id, request.MatterId, request.UserId);
            if (existing == null) return null;

            existing.Description = request.Description;
            existing.Category = request.Category;
            existing.Quantity = request.Quantity;
            existing.Date = request.Date;
            existing.Rate = request.Rate;
            existing.Status = request.Status;

            await _repository.UpdateExpenseEntry(existing);

            return new ExpenseEntryDto
            {
                Id = existing.id,
                UserId = existing.UserId,
                Description = existing.Description,
                Category = existing.Category,
                Quantity = existing.Quantity,
                Date = existing.Date,
                Rate = existing.Rate,
                Amount = existing.Amount,
                
            };
        }
    }
}
