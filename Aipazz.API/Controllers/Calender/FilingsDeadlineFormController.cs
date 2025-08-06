using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.FilingsDeadlineForm.Queries;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using AIpazz.Infrastructure.Jobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Aipazz.API.Controllers.Calendar
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilingsDeadlineFormController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISchedulerFactory _schedulerFactory;

        public FilingsDeadlineFormController(IMediator mediator,  ISchedulerFactory schedulerFactory)
        {
            _mediator = mediator;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string? userId = User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    ?.Value;
                Console.WriteLine("User Id :" + userId);
                var result = await _mediator.Send(new GetAllFilingsDeadlineFormsQuery(userId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                var result = await _mediator.Send(new GetFilingsDeadlineFormByIdQuery(id));
                if (result == null)
                    return NotFound($"Filing deadline with ID {id} not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddFilingsDeadlineFormCommand command)
        {
            try
            {
                // Basic validation
                if (command == null)
                    return BadRequest("Command cannot be null");
                    
                if (string.IsNullOrWhiteSpace(command.Title))
                    return BadRequest("Title is required");
                    
                
                    
                if (command.Date == default(DateTime))
                    return BadRequest("Valid date is required");
                
                string? userId = User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    ?.Value;
                Console.WriteLine("userId: " + userId);
                command.UserId = userId;

                var result = await _mediator.Send(command);
                
                // scheduled an email for filling deadline to client
                if (!string.IsNullOrEmpty(command.UserEmail))
                {
                    var scheduler = await _schedulerFactory.GetScheduler();

                    var jobData = new JobDataMap
                    {
                        { "email", command.UserEmail },
                        { "subject", $"Reminder: Upcoming Deadline - {command.Title}" },
                        { "body", $@"
                        <h3>This is a reminder for your Deadline</h3>
                        <p><strong>Title:</strong> {command.Title}</p>
                        <p><strong>Date:</strong> {command.Date:dddd, MMM dd, yyyy}</p>
                        <p><strong>Time:</strong> {command.Time}</p>
                        <p><strong>Assigned matter:</strong> {command.AssignedMatter}</p>
                        {(string.IsNullOrWhiteSpace(command.Description) ? "" : $"<p><strong>Note:</strong> {command.Description}</p>")}
                    "}
                    };

                    var job = JobBuilder.Create<FillingDeadlineReminder>() // Ensure this job implements IJob
                        .UsingJobData(jobData)
                        .WithIdentity($"reminder_{result.id}", "filling_deadline_reminder")
                        .Build();

                    DateTime reminderTime = command.Reminder; // this must be implement in order to request

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"trigger_{result.id}", "filling_deadline_reminder")
                        .StartAt(reminderTime.ToUniversalTime()) // must be UTC
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);
                    Console.WriteLine("Filling deadLine reminder scheduled.");
                }
                
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateFilingsDeadlineFormCommand command)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                if (command == null)
                    return BadRequest("Command cannot be null");
                    
                if (id != command.Id) 
                    return BadRequest("ID mismatch between URL and request body");
                    
                // Basic validation
                if (string.IsNullOrWhiteSpace(command.Title))
                    return BadRequest("Title is required");
                    
                if (string.IsNullOrWhiteSpace(command.AssignedMatter))
                    return BadRequest("AssignedMatter is required");
                    
                if (command.Date == default(DateTime))
                    return BadRequest("Valid date is required");

                var result = await _mediator.Send(command);

                if (result == null) 
                    return NotFound($"Filing deadline with ID {id} not found");
                    
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                var result = await _mediator.Send(new DeleteFilingsDeadlineFormCommand(id));

                if (!result) 
                    return NotFound($"Filing deadline with ID {id} not found");
                    
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}