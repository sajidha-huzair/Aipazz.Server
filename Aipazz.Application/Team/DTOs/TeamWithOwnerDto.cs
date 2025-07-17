using System;
using System.Collections.Generic;
using Aipazz.Domian.Team;

namespace Aipazz.Application.Team.DTOs
{
    public class TeamWithOwnerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty; // Owner's display name
        public List<TeamMember> Members { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public int MemberCount { get; set; }
    }
}