using Aipazz.Application.Calender.Interface;
using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.clientmeeting.Handlers
{
    public class UpdateClientMeetingHandler : IRequestHandler<UpdateClientMeetingCommand, ClientMeeting>
    {
        private readonly IclientmeetingRepository _repository;

        public UpdateClientMeetingHandler(IclientmeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClientMeeting> Handle(UpdateClientMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await _repository.GetClientMeetingByID(request.Id);
            if (meeting == null) return null!;

            // Update manually since properties are private set
            meeting.UpdateDetails(
                request.Title,
                request.Date,
                request.Time,
                request.Reminder,
                request.Description,
                request.MeetingLink,
                request.Location,
                request.TeamMembers,
                request.ClientEmails
            );
            
            // ✅ Save changes to Cosmos DB
            await _repository.UpdateClientMeeting(meeting);

            return meeting;
        }
    }
}