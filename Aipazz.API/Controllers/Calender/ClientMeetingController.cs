
using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.clientmeeting.queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AIpazz.Infrastructure.Jobs;
using Microsoft.AspNetCore.Authorization;
using Quartz;
using System.Linq.Expressions;


namespace Aipazz.API.Controllers.Calendar
{
    [ApiController] // <-- Important
    [Route("api/[controller]")] // <-- Important
    public class ClientMeetingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICalenderEmailService _calenderEmailService;
        private readonly ISchedulerFactory _schedulerFactory;
        public ClientMeetingController(IMediator mediator, ICalenderEmailService calenderEmailService,  ISchedulerFactory schedulerFactory)
        {
            _mediator = mediator;
            _calenderEmailService = calenderEmailService;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClientMeetings()
        {
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine(userId);
            if (userId == null) return BadRequest();
            var results = await _mediator.Send(new GetAllClientMeetingsquery(userId));
            return Ok(results);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClientMeeting( CreateClientMeetingCommand command)
        {
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("userId: " + userId);
            command.UserId = userId;

            var meeting = await _mediator.Send(command);
            
            // send email to the client
            await _calenderEmailService.sendEmaiToClient(command.ClientEmail ,command.Title, EmailTemplate.WelcomeBody(command.Title,command.Date,new TimeOnly(10,30),command.MeetingLink,command.Location));
            
            // 3. Schedule reminder email using Quartz (based on request.Reminder)
            if (!string.IsNullOrEmpty(command.ClientEmail) && command.Reminder > TimeSpan.Zero)
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                var jobData = new JobDataMap
                {
                    { "email", command.ClientEmail },
                    { "subject", "Reminder: Upcoming Client Meeting" },
                    { "body", $"You have a meeting titled '{command.Title}' scheduled on {command.Date:dddd, MMM dd, yyyy} at {command.Time}." }
                };

                IJobDetail job = JobBuilder.Create<ClientMeetingReminderJob>()
                    .UsingJobData(jobData)
                    .WithIdentity($"reminder_{meeting.Id}", "email_reminders")
                    .Build();

                DateTime reminderTime = DateTime.Now.AddMinutes(1); // this must be implement using request
                
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger_{meeting.Id}", "email_reminders")
                    .StartAt(reminderTime)  
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
                Console.WriteLine("client meeting reminder scheduled");
            }

            return CreatedAtAction(nameof(GetClientMeetings), new { id = meeting.Id }, meeting);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMeetingById(Guid id)
        {
            var meeting = await _mediator.Send(new GetClientMeetingByIdQuery(id));
            
            if (meeting == null)
            {
                return NotFound(); // 404 if not found
            }

            return Ok(meeting); // 200 OK with meeting details
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClientMeeting(Guid id, [FromBody] UpdateClientMeetingCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch between URL and body.");

            var updatedMeeting = await _mediator.Send(command);

            if (updatedMeeting == null)
                return NotFound();

            return Ok(updatedMeeting);
        }
        
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMeeting(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteClientMeetingCommand(id));

            if (!deleted)
            {
                return NotFound(); // 404 if not found
            }

            return NoContent(); // 204 No Content (successfully deleted)
        }


        
    }
}