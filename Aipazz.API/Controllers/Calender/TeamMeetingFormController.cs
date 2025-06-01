using Aipazz.Application.Calender.TeamMeeting.Commands;
using Aipazz.Application.Calender.TeamMeeting.Queries;
using Aipazz.Application.Calender.TeamMeetingForm.Commands;
using Aipazz.Application.Calender.TeamMeetingForms.Queries;
using Aipazz.Domian.Calender;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calender
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamMeetingFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamMeetingFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamMeetingForm>>> GetAll()
        {
            var query = new GetAllTeamMeetingFormsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTeamMeetingFormByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTeamMeetingFormCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTeamMeetingFormCommand command)
        {
            if (id != command.Id)
                return BadRequest("Mismatched ID");

            var result = await _mediator.Send(command);
            if (result == null) return NotFound();

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTeamMeetingFormCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }





    }
}