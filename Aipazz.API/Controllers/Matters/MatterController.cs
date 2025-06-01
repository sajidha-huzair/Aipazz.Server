using Aipazz.Application.Matters.DTO;
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
        public async Task<IActionResult> GetById(string id, string ClientNic)
        {
            var result = await _mediator.Send(new GetMatterByIdQuery(id, ClientNic));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/Matter
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatterCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { Id = result.id, clientNic = result.ClientNic }, result);
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
        public async Task<IActionResult> Delete(string id, string ClientNic)
        {
            var result = await _mediator.Send(new DeleteMatterCommand { Id = id, ClientNic = ClientNic });
            if (!result) return NotFound();
            return NoContent();
        }

        // API/Controllers/MatterController.cs
        [HttpGet("titles")]
        public async Task<IActionResult> GetMatterTitles()
        {
            var result = await _mediator.Send(new GetAllMatterTitlesQuery());
            return Ok(result);
        }

        // PUT: api/Matter/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
        string id,
        [FromQuery] string clientNic,
        [FromBody] UpdateMatterStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewStatusId))
                return BadRequest("New status ID must be provided.");

            await _mediator.Send(new UpdateMatterStatusCommand(id, clientNic, dto.NewStatusId));
            return NoContent();
        }

        // File: Controllers/MatterController.cs
        [HttpGet("status/{statusId}")]
        public async Task<IActionResult> GetMattersByStatusId(string statusId)
        {
            var result = await _mediator.Send(new GetMattersByStatusIdQuery(statusId));
            return Ok(result);
        }






    }
}
