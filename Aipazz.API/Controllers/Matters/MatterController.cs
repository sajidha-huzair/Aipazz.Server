using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.matter.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Matters
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Matter
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMattersQuery());
            return Ok(result);
        }

        // GET: api/Matter/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, string title)
        {
            var result = await _mediator.Send(new GetMatterByIdQuery(id, title));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/Matter
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatterCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { Id = result.id, Title = result.title }, result);
        }

        // PUT: api/Matter/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateMatterCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/Matter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string title)
        {
            var result = await _mediator.Send(new DeleteMatterCommand { Id = id, Title = title });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
