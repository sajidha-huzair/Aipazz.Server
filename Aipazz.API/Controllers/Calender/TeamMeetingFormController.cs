using Aipazz.Application.Calender.TeamMeeting.Commands;
using Aipazz.Application.Calender.TeamMeeting.Queries;

using Aipazz.Application.Calender.TeamMeetingForms.Queries;
using Aipazz.Domian.Calender;
using AIpazz.Infrastructure.Jobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Aipazz.API.Controllers.Calender
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamMeetingFormController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISchedulerFactory _schedulerFactory;

        public TeamMeetingFormController(IMediator mediator,  ISchedulerFactory schedulerFactory)
        {
            _mediator = mediator;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TeamMeetingForm>>> GetAll()
        {
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("userId: " + userId);
            var query = new GetAllTeamMeetingFormsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTeamMeetingFormByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddTeamMeetingFormCommand command)
        {
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("userId: " + userId);
            command.UserId = userId;
            var result = await _mediator.Send(command);

            if (command.TeamMemberEmails != null || !command.TeamMemberEmails.Any())
            {
                foreach (var email in command.TeamMemberEmails)
                {
                    var scheduler = await _schedulerFactory.GetScheduler();

                    var jobData = new JobDataMap
                    {
                        { "email", email },
                        { "subject", $"Reminder: Upcoming team meeting - {command.Title}" },
                        { "body", $@"
                        <h3>This is a reminder for your court date</h3>
                        <p><strong>Title:</strong> {command.Title}</p>
                        <p><strong>Time:</strong> {command.Time}</p>
                        <p><strong>Date:</strong> {command.Date:dddd, MMM dd, yyyy}</p>
                        <p><strong>Video conferencing link:</strong> {command.VideoConferencingLink}</p>
                        <p><strong>Location Link:</strong> {command.LocationLink}</p>
                        {(string.IsNullOrWhiteSpace(command.Description) ? "" : $"<p><strong>Note:</strong> {command.Description}</p>")}
                    "}
                    };

                    var job = JobBuilder.Create<TeamMeetingReminder>() // Ensure this job implements IJob
                        .UsingJobData(jobData)
                        .WithIdentity($"reminder_{result.id}", "team_meeting_reminder")
                        .Build();

                    DateTime reminderTime = DateTime.Now.AddMinutes(1); // this must be implement using request

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"trigger_{result.id}", "team_meeting_reminder")
                        .StartAt(reminderTime.ToUniversalTime()) // must be UTC
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);
                    Console.WriteLine("Team meeting reminder scheduled.");
                }
            }
            
            
            return Ok(result);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTeamMeetingFormCommand command)
        {
            if (id != command.Id)
                return BadRequest("Mismatched ID");

            var result = await _mediator.Send(command);
            if (result == null) return NotFound();

            return Ok(result);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTeamMeetingFormCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
        
    }
}