using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class ClientWithMattersDto
    {
        public string Id { get; set; }=string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Nic { get; set; } = string.Empty;
        public List<MatterWithEntriesDto> Matters { get; set; } = new();
    }

}
