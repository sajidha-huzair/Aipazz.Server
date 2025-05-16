using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DTOs
{
    public class MatterWithEntriesDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }

        public List<TimeEntryDto> TimeEntries { get; set; } = new();
        public List<ExpenseEntryDto> ExpenseEntries { get; set; } = new();
    }

}
