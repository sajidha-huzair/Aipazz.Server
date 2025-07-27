
using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.clientmeeting.queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace Aipazz.API.Controllers.Calendar
{
    [ApiController] // <-- Important
    [Route("api/[controller]")] // <-- Important
    public class ClientMeetingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        public ClientMeetingController(IMediator mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
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

            await _emailService.sendEmaiToClient(command.ClientEmail ,command.Title, EmailTemplate.WelcomeBody(command.Title,command.Date,new TimeOnly(10,30),command.MeetingLink,command.Location));
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
        [Authorize]
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
        [Authorize]
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