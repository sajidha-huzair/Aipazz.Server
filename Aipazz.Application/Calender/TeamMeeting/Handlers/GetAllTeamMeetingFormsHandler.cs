using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeetingForms.Queries;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeetingForms.Handlers
{
    public class GetAllTeamMeetingFormsHandler : IRequestHandler<GetAllTeamMeetingFormsQuery, List<TeamMeetingForm>>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public GetAllTeamMeetingFormsHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public Task<List<TeamMeetingForm>> Handle(GetAllTeamMeetingFormsQuery request, CancellationToken cancellationToken)
        {
            var data = _repository.GetAll();
            return Task.FromResult(data);
        }
    }
}