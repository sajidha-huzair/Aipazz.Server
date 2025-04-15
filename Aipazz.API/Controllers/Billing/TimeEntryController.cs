using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.TimeEntries.Commands;
using Aipazz.Application.Billing.TimeEntries.Queries;
using Aipazz.Domian.Billing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntryController : ControllerBase
    {
       
        private readonly IMediator _mediator;  //Declare IMediator

        // Inject MediatR via Constructor
        public TimeEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/TimeEntry
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTimeEntriesQuery());
            return Ok(result);
        }

        // GET: api/TimeEntry/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, string matterId)
        {
            var result = await _mediator.Send(new GetTimeEntryByIdQuery(id, matterId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/TimeEntry
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimeEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { Id = result.id, MatterId = result.matterId }, result);
        }

        // PUT: api/TimeEntry/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateTimeEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/TimeEntry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string matterId)
        {
            var result = await _mediator.Send(new DeleteTimeEntryCommand { Id = id, MatterId = matterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
