using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.CourtDateForms.Queries;
using Aipazz.Application.Calender.Interface;
using AIpazz.Infrastructure.Jobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Aipazz.API.Controllers.Calender
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateFormController(IMediator mediator, ICalenderEmailService calenderEmailService, ISchedulerFactory schedulerFactory) : ControllerBase
    {
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            string? UserId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("UserId: " + UserId);
            
            var result = await mediator.Send(new GetCourtDateFormListQuery(UserId));
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetCourtDateFormByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCourtDateFormCommand command)
        {
            string? UserId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("UserId: " + UserId);
            command.UserId = UserId;
            var result = await mediator.Send(command);

            // send creation court date to the client
            await calenderEmailService.SendCourtDateEmailToClientAsync(command.ClientEmail, command.Title,
                command.CourtType, command.Stage, command.CourtDate, command.Reminder, command.Note);

            // scheduled reminder for client
            if (!string.IsNullOrEmpty(command.ClientEmail) && command.Reminder > TimeSpan.Zero)
            {
                var scheduler = await schedulerFactory.GetScheduler();

                var jobData = new JobDataMap
                {
                    { "email", command.ClientEmail },
                    { "subject", $"Reminder: Upcoming Court Date - {command.Title}" },
                    { "body", $@"
                        <h3>This is a reminder for your court date</h3>
                        <p><strong>Title:</strong> {command.Title}</p>
                        <p><strong>Date:</strong> {command.CourtDate:dddd, MMM dd, yyyy}</p>
                        <p><strong>Court Type:</strong> {command.CourtType}</p>
                        <p><strong>Stage:</strong> {command.Stage}</p>
                        {(string.IsNullOrWhiteSpace(command.Note) ? "" : $"<p><strong>Note:</strong> {command.Note}</p>")}
                    "}
                };

                var job = JobBuilder.Create<CourtDateReminder>() // Ensure this job implements IJob
                    .UsingJobData(jobData)
                    .WithIdentity($"reminder_{result.id}", "courtdate_reminders")
                    .Build();

                DateTime reminderTime = DateTime.Now.AddMinutes(1); // this must be implement according to the request

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger_{result.id}", "courtdate_reminders")
                    .StartAt(reminderTime.ToUniversalTime()) // must be UTC
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
                Console.WriteLine("Court date reminder scheduled.");
            }

            
            return Ok(result);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCourtDateForm([FromRoute]Guid id, [FromBody]UpdateCourtDateFormCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL and body do not match.");

            command.Id = id;
            var result = await mediator.Send(command);
            if (result == null) return NotFound();

            return Ok(result);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteCourtDateFormCommand(id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}