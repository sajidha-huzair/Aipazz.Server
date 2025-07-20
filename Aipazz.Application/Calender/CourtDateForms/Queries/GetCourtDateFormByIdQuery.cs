using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Queries
{
    public class GetCourtDateFormByIdQuery : IRequest<Domian.Calender.CourtDateForm?>
    {
        public Guid Id { get; set; }

        public GetCourtDateFormByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}