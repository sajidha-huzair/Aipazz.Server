using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.CourtDateForms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calender
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateFormController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await mediator.Send(new GetCourtDateFormListQuery());
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
        public async Task<IActionResult> Create(CreateCourtDateFormCommand command)
        {
            var result = await mediator.Send(command);
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