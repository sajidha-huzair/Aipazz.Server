using System;
using System.Collections.Generic;

namespace Aipazz.Domian.Team
{
    public class Team
    {
        public string id { get; set; } = string.Empty; // Changed to lowercase
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty; // User who created the team
        public List<TeamMember> Members { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
