using Aipazz.Domian.Team;

namespace Aipazz.Application.Team.DTOs
{
    public class AddTeamMemberDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public TeamRole Role { get; set; } = TeamRole.Member;
    }
}