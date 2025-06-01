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
    public class GetClientsWithEntriesQueryHandler : IRequestHandler<GetClientsWithEntriesQuery, List<ClientWithMattersDto>>
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMatterRepository _matterRepo;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;

        public GetClientsWithEntriesQueryHandler(
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

        public async Task<List<ClientWithMattersDto>> Handle(GetClientsWithEntriesQuery request, CancellationToken cancellationToken)
        {
            var allMatters = await _matterRepo.GetAllMatters(request.UserId); // You may want to filter matters by user later
            var result = new List<ClientWithMattersDto>();

            var clientNicToMattersWithEntries = new Dictionary<string, List<MatterWithEntriesDto>>();

            foreach (var matter in allMatters)
            {
                var timeEntries = await _timeRepo.GetTimeEntriesByMatterIdAsync(matter.id!, request.UserId); 
                var expenseEntries = await _expenseRepo.GetExpenseEntriesByMatterIdAsync(matter.id!,request.UserId);

                if (!timeEntries.Any() && !expenseEntries.Any())
                    continue;

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

                if (!clientNicToMattersWithEntries.ContainsKey(matter.ClientNic!))
                    clientNicToMattersWithEntries[matter.ClientNic!] = new();

                clientNicToMattersWithEntries[matter.ClientNic!].Add(matterDto);
            }

            foreach (var (clientNic, matters) in clientNicToMattersWithEntries)
            {
                var client = await _clientRepo.GetByNicAsync(clientNic);
                if (client == null) continue;

                result.Add(new ClientWithMattersDto
                {
                    Id = client.id!,
                    Name = client.name!,
                    Nic = client.nic!,
                    Matters = matters
                });
            }

            return result;
        }
    }

    }
