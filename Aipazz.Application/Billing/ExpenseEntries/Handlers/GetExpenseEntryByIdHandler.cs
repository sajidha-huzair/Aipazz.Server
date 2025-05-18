using Aipazz.Application.Billing.ExpenseEntries.Queries;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.DTOs;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Handlers
{
    public class GetExpenseEntryByIdHandler : IRequestHandler<GetExpenseEntryByIdQuery, ExpenseEntryDto>
    {
        private readonly IExpenseEntryRepository _repository;

        public GetExpenseEntryByIdHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntryDto> Handle(GetExpenseEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var entry = await _repository.GetExpenseEntryById(request.Id, request.MatterId, request.UserId);
            if (entry == null) return null;

            return new ExpenseEntryDto
            {
                Id = entry.id,
                UserId = entry.UserId,
                Description = entry.Description,
                Category = entry.Category,
                Quantity = entry.Quantity,
                Date = entry.Date,
                Rate = entry.Rate,
                Amount = entry.Amount,
                MatterTitle = string.Empty // optionally inject MatterRepository if needed
            };
        }
    }
}
