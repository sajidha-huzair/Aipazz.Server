using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Queries;
using Aipazz.Application.client.Interfaces;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Billing;
using Aipazz.Domian.Matters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class GetClientsWithBilledEntriesHandler
    : IRequestHandler<GetClientsWithBilledEntriesQuery, List<ClientWithMattersDto>>
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMatterRepository _matterRepo;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;

        public GetClientsWithBilledEntriesHandler(
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
            GetClientsWithBilledEntriesQuery request, CancellationToken ct)
        {
            var matters = await _matterRepo.GetAllMatters(request.UserId);
            var map = new Dictionary<string, List<MatterWithEntriesDto>>();

            foreach (var m in matters)
            {
                var time = await _timeRepo.GetBilledByMatterIdAsync(m.id!, request.UserId);
                var expense = await _expenseRepo.GetBilledByMatterIdAsync(m.id!, request.UserId);
                if (!time.Any() && !expense.Any()) continue;   // skip if none billed

                map.TryAdd(m.ClientNic!, new());
                map[m.ClientNic!].Add(BuildMatterDto(m, time, expense));
            }

            var result = new List<ClientWithMattersDto>();
            foreach (var (nic, mtDtos) in map)
            {
                var client = await _clientRepo.GetByNicAsync(nic, request.UserId);
                if (client == null) continue;
                result.Add(new ClientWithMattersDto
                {
                    Id = client.id!,
                    Name = $"{client.FirstName} {client.LastName}".Trim(),
                    Nic = client.nic!,
                    Matters = mtDtos
                });
            }
            return result;
        }

        private static MatterWithEntriesDto BuildMatterDto(
            Matter m, IEnumerable<TimeEntry> time, IEnumerable<ExpenseEntry> exp) =>
            new()
            {
                Id = m.id!,
                Title = m.title,
                CaseNumber = m.CaseNumber,
                Date = m.Date ?? DateTime.Today,
                Description = m.Description,
                TimeEntries = time.Select(t => new TimeEntryDto {
                    Id = t.id!,
                    Description = t.Description,
                    Date = t.Date,
                    Duration = t.Duration,
                    RatePerHour = t.RatePerHour,  
                    Amount = t.Amount
                }).ToList(),
                ExpenseEntries = exp.Select(e => new ExpenseEntryDto {
                    Id = e.id!,
                    Category = e.Category,
                    Rate = e.Rate,          
                    Quantity = e.Quantity,
                    Amount = e.Amount,          
                    Date = e.Date
                }).ToList()
            };
    }

}
