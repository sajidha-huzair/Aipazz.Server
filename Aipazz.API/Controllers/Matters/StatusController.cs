using Aipazz.Application.Matters.matterStatus.Commands;
using Aipazz.Application.Matters.matterStatus.Queries;
using Aipazz.Infrastructure.Matters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers.Matters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication
    public class StatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly StatusSeeder _statusSeeder;

        public StatusController(IMediator mediator, StatusSeeder statusSeeder)
        {
            _mediator = mediator;
            _statusSeeder = statusSeeder;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   User.FindFirstValue("sub") ??
                   throw new UnauthorizedAccessException("UserId not found in token.");
        }

        // GET: api/Status
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();  // Get the userId from JWT token

            // Seed default statuses if they don’t exist
            await _statusSeeder.SeedDefaultStatusesAsync(userId);

            var result = await _mediator.Send(new GetAllStatusesQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = GetUserId();  // Get the userId from JWT token
            var result = await _mediator.Send(new GetStatusByIdQuery(id, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStatusCommand command)
        {
            if (command == null) return BadRequest("Invalid status data.");

            var userId = GetUserId();  // Get the userId from JWT token
            command.UserId = userId;

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateStatusCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid update request.");

            var userId = GetUserId();  // Get the userId from JWT token
            command.UserId = userId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();  // Get the userId from JWT token
            var success = await _mediator.Send(new DeleteStatusCommand(id, userId));
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
