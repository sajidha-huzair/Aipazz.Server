using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Application.Matters.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientWithDetailsQueryHandler : IRequestHandler<GetClientWithDetailsQuery, ClientWithMattersDto>
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMatterRepository _matterRepo;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;

        public GetClientWithDetailsQueryHandler(
            IClientRepository clientRepo,
            IMatterRepository matterRepo,
            ITimeEntryRepository timeRepo,
            IExpenseEntryRepository expenseRepo)
        {
            _clientRepo = clientRepo;
            _matterRepo = matterRepo;
            _timeRepo = timeRepo;
            _expenseRepo = expenseRepo;
        }

        public async Task<ClientWithMattersDto?> Handle(GetClientWithDetailsQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientRepo.GetByNicAsync(request.ClientNic);
            if (client == null) return null;

            var matters = await _matterRepo.GetMattersByClientNicAsync(request.ClientNic, request.UserId);

            var result = new ClientWithMattersDto
            {
                Id = client.id!,
                Name = $"{client.FirstName} {client.LastName}".Trim(),
                Nic = client.nic!,
                Matters = new()
            };

            foreach (var matter in matters)
            {
                var timeEntries = await _timeRepo.GetTimeEntriesByMatterIdAsync(matter.id!, request.UserId);
                var expenseEntries = await _expenseRepo.GetExpenseEntriesByMatterIdAsync(matter.id!, request.UserId);

                var matterDto = new MatterWithEntriesDto
                {
                    Id = matter.id!,
                    Title = matter.title,
                    CaseNumber = matter.CaseNumber,
                    Date = matter.Date ?? DateTime.Today,
                    Description = matter.Description,
                    TimeEntries = timeEntries.Select(t => new TimeEntryDto


                    {
                        Id = t.id!,
                        Description = t.Description,
                        Date = t.Date == default ? DateTime.Today : t.Date,
                        Duration = t.Duration,
                        RatePerHour = t.RatePerHour,
                        Amount = t.Amount
                    }).ToList(),
                    ExpenseEntries = expenseEntries.Select(e => new ExpenseEntryDto
                    {
                        Id = e.id!,
                        Category = e.Category,
                        Rate = e.Rate,
                        Quantity = e.Quantity,
                        Amount = e.Rate * e.Quantity,
                        Date = e.Date == default ? DateTime.Today : e.Date
                    }).ToList()
                };

                result.Matters.Add(matterDto);
            }

            return result;
        }
    }

}
