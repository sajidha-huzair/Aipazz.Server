using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.courtdate.queries
{
    public class GetAllCourtDatesQuery : IRequest<List<CourtDateForm>>
    {
    }
}