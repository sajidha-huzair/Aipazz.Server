
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
        public async Task<IActionResult> CreateClientMeeting([FromBody] CreateClientMeetingCommand command)
        {
            var meeting = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClientMeetings), new { id = meeting.Id }, meeting);
        }
        
        
        
        
    }
}