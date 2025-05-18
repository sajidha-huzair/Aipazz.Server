using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;


namespace Aipazz.Application.Billing.ExpenseEntries.Queries
{
    public class GetAllExpenseEntriesQuery : IRequest<List<ExpenseEntryDto>>
    {
        public string UserId { get; }

        public GetAllExpenseEntriesQuery(string userId)
        {
            UserId = userId;
        }
    }
}
