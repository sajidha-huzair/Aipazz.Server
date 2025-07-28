using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calendar.CourtDateForms.Queries
{
    public record GetCourtDateFormListQuery(string UserId) : IRequest<List<CourtDateForm>>;

}