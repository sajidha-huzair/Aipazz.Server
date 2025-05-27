using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Matters
{
    public class Status
    {
        public required string Id { get; set; }  // Cosmos DB ID
        public string Name { get; set; } = string.Empty; // e.g., "To Do", "In Progress"
    }
}
