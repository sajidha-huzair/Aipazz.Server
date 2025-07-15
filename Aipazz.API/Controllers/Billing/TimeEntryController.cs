using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Domian.Billing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeEntryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITimeEntryRepository _timeRepo;

        public TimeEntryController(IMediator mediator, ITimeEntryRepository timeRepo)
        {
            _mediator = mediator;
            _timeRepo = timeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new GetAllTimeEntriesQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string matterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new GetTimeEntryByIdQuery(id, matterId, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimeEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, matterId = command.MatterId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTimeEntryCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid request.");
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] string matterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new DeleteTimeEntryCommand
            {
                Id = id,
                MatterId = matterId,
                UserId = userId
            });

            return result ? NoContent() : NotFound();
        }

        [HttpPost("by-ids")]
        public async Task<IActionResult> GetByIds([FromBody] List<string> entryIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeRepo.GetAllEntriesByIdsAsync(entryIds, userId);
            return Ok(result);
        }
    }
}
