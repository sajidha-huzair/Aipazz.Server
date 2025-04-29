using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers;

public class CreateClientMeetingHandler : IRequestHandler<CreateClientMeetingCommand, ClientMeeting>
{
    private readonly IclientmeetingRepository _repository;

    public CreateClientMeetingHandler(IclientmeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClientMeeting> Handle(CreateClientMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = new ClientMeeting(
            request.Id,
            request.Title,
            request.Date,
            request.Time,
            request.Repeat,
            request.Reminder,
            request.Description,
            request.MeetingLink,
            request.Location,
            request.TeamMembers,
            request.ClientEmail
        );

        await _repository.AddClientMeeting(meeting);
        return meeting;
    }
}