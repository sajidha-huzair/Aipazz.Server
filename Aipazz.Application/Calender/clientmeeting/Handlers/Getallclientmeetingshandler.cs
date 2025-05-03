using Aipazz.Application.Calender.clientmeeting.queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers;

public class Getallclientmeetingshandler:IRequestHandler<GetAllClientMeetingsquery,List<ClientMeeting>>
{
    private readonly IclientmeetingRepository _repository;

    public Getallclientmeetingshandler(IclientmeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ClientMeeting>> Handle(GetAllClientMeetingsquery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllClientMeetings();
    }
        

}