using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Domian.Matters
{
    public class MatterType
    {
        public required string id { get; set; }  
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}

