using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.Queries.FilingsDeadlineForms
{
    public class GetFilingsDeadlineFormByIdQuery : IRequest<Domian.Calender.FilingsDeadlineForm?>
    {
        public Guid Id { get; set; }

        public GetFilingsDeadlineFormByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}