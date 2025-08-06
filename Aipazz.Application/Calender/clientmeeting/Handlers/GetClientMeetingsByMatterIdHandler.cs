using Aipazz.Application.Calender.clientmeeting.Queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers
{
    public class GetClientMeetingsByMatterIdHandler : IRequestHandler<GetClientMeetingsByMatterIdQuery, List<ClientMeeting>>
    {
        private readonly IclientmeetingRepository _repository;

        public GetClientMeetingsByMatterIdHandler(IclientmeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClientMeeting>> Handle(GetClientMeetingsByMatterIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMeetingsByMatterIdAsync(request.MatterId, request.UserId);
        }
    }

}
