using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.Interface;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers
{
    public class DeleteClientMeetingHandler : IRequestHandler<DeleteClientMeetingCommand, bool>
    {
        private readonly IclientmeetingRepository _repository;

        public DeleteClientMeetingHandler(IclientmeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteClientMeetingCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteClientMeeting(request.Id);
        }
    }
}