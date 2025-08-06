using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.TeamMeetingForms.Queries
{
    public record GetAllTeamMeetingFormsQuery(string UserId) : IRequest<List<Domian.Calender.TeamMeetingForm>> { }
}