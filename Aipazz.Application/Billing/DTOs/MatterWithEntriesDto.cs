using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class MatterWithEntriesDto
    {
        public string Id { get; set; }=string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CaseNumber { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Description { get; set; } = string.Empty;

        public List<TimeEntryDto> TimeEntries { get; set; } = new();
        public List<ExpenseEntryDto> ExpenseEntries { get; set; } = new();
    }

}
