using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Team;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AIpazz.Infrastructure.Team
{
    public class TeamRepository : ITeamRepository
    {
        private readonly Container _container;

        public TeamRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["Team"]; 
            _container = db.GetContainer(containerName);
        }

        public async Task<string> CreateTeamAsync(Aipazz.Domian.Team.Team team)
        {
            await _container.CreateItemAsync(team, new PartitionKey(team.OwnerId));
            return team.id;
        }

        public async Task DeleteTeamAsync(string teamId, string ownerId)
        {
            await _container.DeleteItemAsync<Aipazz.Domian.Team.Team>(teamId, new PartitionKey(ownerId));
        }

        public async Task<List<Aipazz.Domian.Team.Team>> GetAllTeamsByOwnerIdAsync(string ownerId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.OwnerId = @ownerId AND c.IsActive = true")
                .WithParameter("@ownerId", ownerId);

            var iterator = _container.GetItemQueryIterator<Aipazz.Domian.Team.Team>(query);
            var teams = new List<Aipazz.Domian.Team.Team>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                teams.AddRange(response);
            }

            return teams;
        }

        public async Task<Aipazz.Domian.Team.Team?> GetTeamByIdAsync(string teamId, string userId)
        {
            // Check if user is owner
            try
            {
                var response = await _container.ReadItemAsync<Aipazz.Domian.Team.Team>(teamId, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // If not owner, check if user is a member
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @teamId AND ARRAY_CONTAINS(c.Members, {'UserId': @userId}, true)")
                    .WithParameter("@teamId", teamId)
                    .WithParameter("@userId", userId);

                var iterator = _container.GetItemQueryIterator<Aipazz.Domian.Team.Team>(query);
                
                if (iterator.HasMoreResults)
                {
                    var queryResponse = await iterator.ReadNextAsync();
                    return queryResponse.FirstOrDefault();
                }
                
                return null;
            }
        }

        public async Task<List<Aipazz.Domian.Team.Team>> GetTeamsByUserIdAsync(string userId)
        {
            // Get teams where user is owner or member
            var query = new QueryDefinition(@"
                SELECT * FROM c 
                WHERE c.IsActive = true 
                AND (c.OwnerId = @userId OR ARRAY_CONTAINS(c.Members, {'UserId': @userId}, true))")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<Aipazz.Domian.Team.Team>(query);
            var teams = new List<Aipazz.Domian.Team.Team>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                teams.AddRange(response);
            }

            return teams;
        }

        public async Task<bool> IsTeamMemberAsync(string teamId, string userId)
        {
            var query = new QueryDefinition(@"
                SELECT VALUE COUNT(1) FROM c 
                WHERE c.id = @teamId 
                AND (c.OwnerId = @userId OR ARRAY_CONTAINS(c.Members, {'UserId': @userId}, true))")
                .WithParameter("@teamId", teamId)
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<int>(query);
            
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault() > 0;
            }

            return false;
        }

        public async Task UpdateTeamAsync(Aipazz.Domian.Team.Team team)
        {
            await _container.UpsertItemAsync(team, new PartitionKey(team.OwnerId));
        }

        public async Task<List<TeamMember>> GetTeamMembersAsync(string teamId, string userId)
        {
            var team = await GetTeamByIdAsync(teamId, userId);
            if (team == null) return new List<TeamMember>();
            
            return team.Members.Where(m => m.IsActive).ToList();
        }

        public async Task<TeamMember?> GetTeamMemberByIdAsync(string teamId, string memberId, string userId)
        {
            var team = await GetTeamByIdAsync(teamId, userId);
            if (team == null) return null;
            
            return team.Members.FirstOrDefault(m => m.Id == memberId && m.IsActive);
        }

        public async Task<TeamMember> AddTeamMemberAsync(string teamId, TeamMember member, string userId)
        {
            var team = await GetTeamByIdAsync(teamId, userId);
            if (team == null) throw new KeyNotFoundException("Team not found");
            
            // Check if member already exists
            var existingMember = team.Members.FirstOrDefault(m => m.UserId == member.UserId);
            if (existingMember != null)
            {
                throw new InvalidOperationException("User is already a member of this team");
            }
            
            team.Members.Add(member);
            team.LastModifiedAt = DateTime.UtcNow;
            
            await UpdateTeamAsync(team);
            return member;
        }

        public async Task UpdateTeamMemberAsync(string teamId, string memberId, TeamMember updatedMember, string userId)
        {
            var team = await GetTeamByIdAsync(teamId, userId);
            if (team == null) throw new KeyNotFoundException("Team not found");
            
            var existingMember = team.Members.FirstOrDefault(m => m.Id == memberId);
            if (existingMember == null) throw new KeyNotFoundException("Team member not found");
            
            existingMember.FirstName = updatedMember.FirstName;
            existingMember.LastName = updatedMember.LastName;
            existingMember.Role = updatedMember.Role;
            existingMember.IsActive = updatedMember.IsActive;
            
            team.LastModifiedAt = DateTime.UtcNow;
            await UpdateTeamAsync(team);
        }

        public async Task DeleteTeamMemberAsync(string teamId, string memberId, string userId)
        {
            var team = await GetTeamByIdAsync(teamId, userId);
            if (team == null) throw new KeyNotFoundException("Team not found");
            
            var member = team.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null) throw new KeyNotFoundException("Team member not found");
            
            team.Members.Remove(member);
            team.LastModifiedAt = DateTime.UtcNow;
            
            await UpdateTeamAsync(team);
        }
    }
}