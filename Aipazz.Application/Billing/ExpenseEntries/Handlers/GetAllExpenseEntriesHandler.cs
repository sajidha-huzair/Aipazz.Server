using Aipazz.Application.Billing.ExpenseEntries.Queries;
using Aipazz.Application.Interfaces;
using Aipazz.Domian.Billing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.ExpenseEntries.Handlers
{
    public class GetAllExpenseEntriesHandler : IRequestHandler<GetAllExpenseEntriesQuery, List<ExpenseEntry>>
    {
        private readonly IExpenseEntryRepository _repository;

        public GetAllExpenseEntriesHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpenseEntry>> Handle(GetAllExpenseEntriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllExpenseEntries();
        }
    }
}
