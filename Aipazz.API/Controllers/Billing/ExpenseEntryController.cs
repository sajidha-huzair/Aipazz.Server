using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.ExpenseEntries.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseEntryController : ControllerBase
    {

        private readonly IMediator _mediator;  //Declare IMediator

        // Inject MediatR via Constructor
        public ExpenseEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/TimeEntry
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllExpenseEntriesQuery());
            return Ok(result);
        }

        // GET: api/TimeEntry/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, string matterId)
        {
            var result = await _mediator.Send(new GetExpenseEntryByIdQuery(id, matterId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/TimeEntry
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { Id = result.id, MatterId = result.matterId }, result);
        }

        // PUT: api/TimeEntry/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateExpenseEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/TimeEntry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string matterId)
        {
            var result = await _mediator.Send(new DeleteExpenseEntryCommand { Id = id, MatterId = matterId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
