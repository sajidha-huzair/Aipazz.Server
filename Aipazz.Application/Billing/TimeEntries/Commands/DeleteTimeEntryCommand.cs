using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.TimeEntries.Commands
{
    public class DeleteTimeEntryCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string MatterId { get; set; } = string.Empty;
    }
}
