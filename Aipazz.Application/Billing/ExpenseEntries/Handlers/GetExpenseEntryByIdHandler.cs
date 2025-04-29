using Aipazz.Application.Billing.ExpenseEntries.Queries;
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
    public class GetExpenseEntryByIdHandler : IRequestHandler<GetExpenseEntryByIdQuery, ExpenseEntry>
    {
        private readonly IExpenseEntryRepository _repository;

        public GetExpenseEntryByIdHandler(IExpenseEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExpenseEntry> Handle(GetExpenseEntryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetExpenseEntryById(request.Id, request.MatterId);
        }
    }
}
