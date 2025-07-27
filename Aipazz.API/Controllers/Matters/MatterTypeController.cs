
using Aipazz.Application.Matters.matterTypes.Commands;
using Aipazz.Application.Matters.matterTypes.Queries;
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
    [Authorize]
    public class MatterTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly MatterTypeSeeder _seeder;

        public MatterTypeController(IMediator mediator, MatterTypeSeeder seeder)
        {
            _mediator = mediator;
            _seeder = seeder;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   User.FindFirstValue("sub") ??
                   throw new UnauthorizedAccessException("UserId not found in token.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            await _seeder.SeedDefaultMatterTypesAsync(userId);
            var result = await _mediator.Send(new GetAllMatterTypesQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetMatterTypeByIdQuery(id, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatterTypeCommand command)
        {
            var userId = GetUserId();
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateMatterTypeCommand command)
        {
            var userId = GetUserId();
            if (command == null || id != command.Id) return BadRequest();
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();
            var success = await _mediator.Send(new DeleteMatterTypeCommand(id, userId));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
