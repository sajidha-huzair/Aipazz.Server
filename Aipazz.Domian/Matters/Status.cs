using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Matters
{
    public class Status
    {
        public required string id { get; set; }  // Partition Key
        public string Name { get; set; } = string.Empty;
    }
}
