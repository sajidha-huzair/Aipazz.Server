using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.queries
{
    public class GetClientMeetingByIdQueryHandler : IRequestHandler<GetClientMeetingByIdQuery, ClientMeeting>
    {
        private readonly IclientmeetingRepository _repository;

        public GetClientMeetingByIdQueryHandler(IclientmeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClientMeeting> Handle(GetClientMeetingByIdQuery request, CancellationToken cancellationToken)
        {
            var meeting = await _repository.GetClientMeetingByID(request.Id);
            return meeting; // Could be null — that's okay, controller will handle.
        }
    }
}