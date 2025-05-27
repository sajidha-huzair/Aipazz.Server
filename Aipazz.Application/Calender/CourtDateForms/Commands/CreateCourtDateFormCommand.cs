using MediatR;
using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.CourtDateForm.Commands
{
    public class CreateCourtDateFormCommand : IRequest<Domian.Calender.CourtDateForm>
    {
        public string CaseNumber { get; set; } = null!;
        public string CourtName { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}