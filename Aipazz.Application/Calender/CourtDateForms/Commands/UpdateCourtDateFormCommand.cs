using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Commands
{
    public class UpdateCourtDateFormCommand : IRequest<Domian.Calender.CourtDateForm?>
    {
        public Guid Id { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string CourtName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}