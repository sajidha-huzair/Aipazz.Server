using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers;

public class CreateClientMeetingHandler : IRequestHandler<CreateClientMeetingCommand, ClientMeeting>
{
    private readonly IclientmeetingRepository _repository;
    private readonly IEmailService _emailService;

    public CreateClientMeetingHandler(IclientmeetingRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<ClientMeeting> Handle(CreateClientMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = new ClientMeeting(
            request.Id,
            request.UserId,
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
        
        // create client meeting
        await _repository.AddClientMeeting(meeting);
        
        // send notifications to the owner and team members assigned to client meeting
        // 2. Send email to team members
        var teamMemberEmails = request.ClientEmails ?? new List<string>();

        if (teamMemberEmails.Any())
        {
            await _emailService.SendClientMeetingEmailToMembersAsync(
                memberEmails: teamMemberEmails,
                meetingTitle: request.Title,
                meetingDate: request.Date,
                meetingTime: request.Time,
                meetingLink: request.MeetingLink,
                ownerEmail: request.UserId // This assumes UserId is the organizer's email — adjust if needed
            );
        }
        return meeting;
    }
}