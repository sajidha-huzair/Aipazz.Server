using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aipazz.Domian.Matters
{
    public class Matter
    {
        
        public required string id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty; 

        public string CaseNumber { get; set; } = string.Empty;

        public DateTime? Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ClientNic { get; set; } = string.Empty;// Partition Key

        public List<string> TeamMembers { get; set; } = new();
        public required string StatusId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CourtType CourtType { get; set; }

    }
}
