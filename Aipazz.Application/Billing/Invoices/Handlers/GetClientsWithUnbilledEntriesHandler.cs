using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Queries;
using Aipazz.Application.client.Interfaces;
using Aipazz.Application.Matters.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class GetClientsWithUnbilledEntriesHandler
    : IRequestHandler<GetClientsWithUnbilledEntriesQuery, List<ClientWithMattersDto>>
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMatterRepository _matterRepo;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;

        public GetClientsWithUnbilledEntriesHandler(
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

        public async Task<List<ClientWithMattersDto>> Handle(
            GetClientsWithUnbilledEntriesQuery request,
            CancellationToken ct)
        {
            // 1. Load all matters (could filter by user up front)
            var matters = await _matterRepo.GetAllMatters(request.UserId);
            var result = new List<ClientWithMattersDto>();

            // 2. Build dictionary keyed by clientNic
            var clientNicToMatters = new Dictionary<string, List<MatterWithEntriesDto>>();

            foreach (var matter in matters)
            {
                var time = await _timeRepo.GetUnbilledByMatterIdAsync(matter.id!, request.UserId);
                var expense = await _expenseRepo.GetUnbilledByMatterIdAsync(matter.id!, request.UserId);

                if (!time.Any() && !expense.Any()) continue;   // Skip if nothing to bill

                var matterDto = new MatterWithEntriesDto
                {
                    Id = matter.id!,
                    Title = matter.title,
                    CaseNumber = matter.CaseNumber,
                    Date = matter.Date ?? DateTime.Today,
                    Description = matter.Description,
                    TimeEntries = time.Select(t => new TimeEntryDto
                    {
                        Id = t.id!,
                        Description = t.Description,
                        Date = t.Date,
                        Duration = t.Duration,
                        RatePerHour = t.RatePerHour,
                        Amount = t.Amount
                    }).ToList(),
                    ExpenseEntries = expense.Select(e => new ExpenseEntryDto
                    {
                        Id = e.id!,
                        Category = e.Category,
                        Rate = e.Rate,
                        Quantity = e.Quantity,
                        Amount = e.Amount,
                        Date = e.Date
                    }).ToList()
                };

                if (!clientNicToMatters.ContainsKey(matter.ClientNic!))
                    clientNicToMatters[matter.ClientNic!] = new();

                clientNicToMatters[matter.ClientNic!].Add(matterDto);
            }

            // 3. Map clients
            foreach (var (nic, matterDtos) in clientNicToMatters)
            {
                var client = await _clientRepo.GetByNicAsync(nic);
                if (client == null) continue;

                result.Add(new ClientWithMattersDto
                {
                    Id = client.id!,
                    Name = $"{client.FirstName} {client.LastName}".Trim(),
                    Nic = client.nic!,
                    Matters = matterDtos
                });
            }

            return result;
        }
    }

}
