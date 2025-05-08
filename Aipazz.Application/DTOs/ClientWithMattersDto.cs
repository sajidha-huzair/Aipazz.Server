using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DTOs
{
    public class ClientWithMattersDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nic { get; set; }
        public List<MatterWithEntriesDto> Matters { get; set; } = new();
    }

}
