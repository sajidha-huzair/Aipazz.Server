using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.matterStatus.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Matters
{
    [ApiController]
    [Route("api/[controller]")]
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

        // GET: api/Status/{id}/{name}
        [HttpGet("{id}/{Name}")]
        public async Task<IActionResult> GetById(string id, string Name)
        {
            var result = await _mediator.Send(new GetStatusByIdQuery(id, Name));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/Status
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStatusCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.Id, name = created.Name }, created);
        }

        // PUT: api/Status/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateStatusCommand command)
        {
            if (command == null || command.Id != id)
                return BadRequest("Invalid request.");

            var updated = await _mediator.Send(command);
            return Ok(updated);
        }

        // DELETE: api/Status/{id}/{name}
        [HttpDelete("{id}/{Name}")]
        public async Task<IActionResult> Delete(string id, string Name)
        {
            var deleted = await _mediator.Send(new DeleteStatusCommand { Id = id, name = Name });
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
