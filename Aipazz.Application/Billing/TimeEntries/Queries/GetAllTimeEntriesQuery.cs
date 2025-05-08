using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Aipazz.Application.DTOs;

namespace Aipazz.Application.Billing.TimeEntries.Queries
{
    public class GetAllTimeEntriesQuery : IRequest<List<TimeEntryDto>>
    {
    }
}
