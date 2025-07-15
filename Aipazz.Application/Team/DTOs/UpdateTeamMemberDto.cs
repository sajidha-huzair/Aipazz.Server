using Aipazz.Domian.Team;

namespace Aipazz.Application.Team.DTOs
{
    public class UpdateTeamMemberDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public TeamRole Role { get; set; } = TeamRole.Member;
        public bool IsActive { get; set; } = true;
    }
}