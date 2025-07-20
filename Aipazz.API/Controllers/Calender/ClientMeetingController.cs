
using Aipazz.Application.Calender.clientmeeting.Commands;
using Aipazz.Application.Calender.clientmeeting.queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController] // <-- Important
    [Route("api/[controller]")] // <-- Important
    public class ClientMeetingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientMeetingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientMeetings()
        {
            var results = await _mediator.Send(new GetAllClientMeetingsquery());
            return Ok(results);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateClientMeeting( CreateClientMeetingCommand command)
        {
            var meeting = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClientMeetings), new { id = meeting.Id }, meeting);
        }
        
        [HttpGet("{id}")]
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
        
        
        [HttpDelete("{id}")]
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