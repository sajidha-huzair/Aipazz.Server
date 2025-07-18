using Aipazz.Domain.Calender;
using MediatR;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Queries.FilingsDeadlineForms
{
    public class GetFilingsDeadlineFormByIdQuery : IRequest<FilingsDeadlineFormEntity?>
    {
        public Guid Id { get; set; }

        public GetFilingsDeadlineFormByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}