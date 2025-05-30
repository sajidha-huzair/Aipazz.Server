using Aipazz.Application.Calendar.CourtDateForms.Commands;
using Aipazz.Application.Calendar.CourtDateForms.queries;
using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.CourtDateForm.Commands;
using Aipazz.Application.Calender.CourtDateForms.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Domian.Calender;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourtDateFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourtDateForm>>> GetAll()
        {
            var result = await _mediator.Send(new GetCourtDateFormListQuery());
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCourtDateFormByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourtDateFormCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCourtDateForm([FromRoute]Guid id, [FromBody]UpdateCourtDateFormCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _mediator.Send(command);
            if (result == null) return NotFound();

            return Ok(result);
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCourtDateFormCommand(id));
            if (!result)
                return NotFound();

            return NoContent();
        }





    }
}