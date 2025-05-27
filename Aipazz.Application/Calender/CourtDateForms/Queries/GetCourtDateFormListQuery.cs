using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calendar.CourtDateForms.Queries
{
    public class GetCourtDateFormListQuery : IRequest<List<CourtDateForm>>
    {
    }
}