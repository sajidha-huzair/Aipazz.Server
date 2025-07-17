using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.DTO
{
    public class MatterDto
    {
        public required string id { get; set; }
        public string title { get; set; } = string.Empty;
        public string CaseNumber { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ClientNic { get; set; } = string.Empty;// Partition Key
        public string StatusId { get; set; } = string.Empty;
        public List<string> TeamMembers { get; set; } = new();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CourtType? CourtType { get; set; }
    }
}
