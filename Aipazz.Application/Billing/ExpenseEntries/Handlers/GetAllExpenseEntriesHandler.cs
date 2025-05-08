using Aipazz.Application.Billing.ExpenseEntries.Queries;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.DTOs;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Handlers
{
    public class GetAllExpenseEntriesHandler : IRequestHandler<GetAllExpenseEntriesQuery, List<ExpenseEntryDto>>
    {
        private readonly IExpenseEntryRepository _repository;
        private readonly IMatterRepository _matterRepository;

        public GetAllExpenseEntriesHandler(IExpenseEntryRepository repository, IMatterRepository matterRepository)
        {
            _repository = repository;
            _matterRepository = matterRepository;
        }

        public async Task<List<ExpenseEntryDto>> Handle(GetAllExpenseEntriesQuery request, CancellationToken cancellationToken)
        {
            var entries = await _repository.GetAllExpenseEntries();
            var matters = await _matterRepository.GetAllMatters();

            return entries.Select(e => new ExpenseEntryDto
            {
                Id = e.id,
                MatterTitle = matters.FirstOrDefault(m => m.id == e.matterId)?.title ?? "",
                Category = e.Category,
                Quantity = e.Quantity,
                Rate = e.Rate,
                Amount = e.Quantity * e.Rate,
                Date = e.Date,
                Description = e.Description
            }).ToList();
        }
    }
}
