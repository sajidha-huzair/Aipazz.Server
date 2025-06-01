using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeeting;
using Aipazz.Application.Calender.TeamMeeting.Queries;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class GetTeamMeetingFormByIdHandler : IRequestHandler<GetTeamMeetingFormByIdQuery, Domian.Calender.TeamMeetingForm?>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public GetTeamMeetingFormByIdHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public Task<Domian.Calender.TeamMeetingForm?> Handle(GetTeamMeetingFormByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetById(request.Id);
        }
    }
}