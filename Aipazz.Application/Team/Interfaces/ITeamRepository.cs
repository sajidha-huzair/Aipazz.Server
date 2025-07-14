using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Team;

namespace Aipazz.Application.Team.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Aipazz.Domian.Team.Team>> GetAllTeamsByOwnerIdAsync(string ownerId);
        Task<Aipazz.Domian.Team.Team?> GetTeamByIdAsync(string teamId, string userId);
        Task<string> CreateTeamAsync(Aipazz.Domian.Team.Team team);
        Task UpdateTeamAsync(Aipazz.Domian.Team.Team team);
        Task DeleteTeamAsync(string teamId, string ownerId);
        Task<bool> IsTeamMemberAsync(string teamId, string userId);
        Task<List<Aipazz.Domian.Team.Team>> GetTeamsByUserIdAsync(string userId);
        
        // Team member specific methods
        Task<List<TeamMember>> GetTeamMembersAsync(string teamId, string userId);
        Task<TeamMember?> GetTeamMemberByIdAsync(string teamId, string memberId, string userId);
        Task<TeamMember> AddTeamMemberAsync(string teamId, TeamMember member, string userId);
        Task UpdateTeamMemberAsync(string teamId, string memberId, TeamMember member, string userId);
        Task DeleteTeamMemberAsync(string teamId, string memberId, string userId);
    }
}