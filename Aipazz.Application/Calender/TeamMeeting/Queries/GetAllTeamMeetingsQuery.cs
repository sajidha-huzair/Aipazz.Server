using Aipazz.Domian.Calender;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Calender.TeamMeetingForms.Queries
{
    public class GetAllTeamMeetingFormsQuery : IRequest<List<TeamMeetingForm>> { }
}