using System.Security.AccessControl;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Commands
{
    public class CreateCourtDateFormCommand : IRequest<Domian.Calender.CourtDateForm>
    {
        public string? UserId { get; set; }
        public string? Title { get; set; }
        public string? CourtType { get; set; }
        public string? Stage { get; set; }
        public List<string>? Clients { get; set; }
        public DateTime CourtDate { get; set; }

        public DateTime Reminder { get; set; } // e.g., 2 or 7 days before CourtDate
        
        public string? Note { get; set; }
        public List<string>? TeamMembers { get; set; }
        public List<string> ClientEmails { get; set; }
        public List<string>? TeamMemberEmails { get; set; }
    }
}