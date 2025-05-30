using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.matterStatus.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers.Matters
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Status
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStatusesQuery());
            return Ok(result);
        }

        // GET: api/Status/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetStatusByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/Status
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStatusCommand command)
        {
            if (command == null) return BadRequest("Invalid status data.");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.id }, result);
        }

        // PUT: api/Status/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateStatusCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid update request.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/Status/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _mediator.Send(new DeleteStatusCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
