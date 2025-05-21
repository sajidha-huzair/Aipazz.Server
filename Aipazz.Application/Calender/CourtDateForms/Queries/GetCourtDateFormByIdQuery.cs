using Aipazz.Domian.Calendar;
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