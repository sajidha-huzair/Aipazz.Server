using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.courtdate.queries

{
    public record GetCourtDateFormByIdQuery(Guid Id) : IRequest<CourtDateForm>;
}
