using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calender.TeamMeeting.Commands;
using MediatR;

namespace Aipazz.Application.Calender.TeamMeeting.Handlers
{
    public class DeleteTeamMeetingFormCommandHandler : IRequestHandler<DeleteTeamMeetingFormCommand, bool>
    {
        private readonly ITeamMeetingFormRepository _repository;

        public DeleteTeamMeetingFormCommandHandler(ITeamMeetingFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTeamMeetingFormCommand request, CancellationToken cancellationToken)
        {
            return await _repository.Delete(request.Id);
        }
    }
}