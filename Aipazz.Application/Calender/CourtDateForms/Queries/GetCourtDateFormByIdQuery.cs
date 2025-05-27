using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calendar.CourtDateForms.queries
{
    public class GetCourtDateFormByIdQuery : IRequest<CourtDateForm?>
    {
        public Guid Id { get; set; }

        public GetCourtDateFormByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}