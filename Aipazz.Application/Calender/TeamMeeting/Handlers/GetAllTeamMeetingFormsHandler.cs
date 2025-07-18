using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeetingForms.Queries;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class GetAllTeamMeetingFormsHandler : IRequestHandler<GetAllTeamMeetingFormsQuery, List<Domian.Calender.TeamMeetingForm>>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public GetAllTeamMeetingFormsHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Aipazz.Domian.Calender.TeamMeetingForm>> Handle(GetAllTeamMeetingFormsQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAll();
            return data;
        }
    }
}