using Aipazz.Application.Billing.ExpenseEntries.Commands;
using Aipazz.Application.Billing.ExpenseEntries.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Aipazz.API.Controllers.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseEntryController : ControllerBase
    {

        private readonly IMediator _mediator;  //Declare IMediator

        // Inject MediatR via Constructor
        public ExpenseEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new GetAllExpenseEntriesQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string matterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new GetExpenseEntryByIdQuery(id, matterId,userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseEntryCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, matterId = command.MatterId }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateExpenseEntryCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid request.");
            command.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string matterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new DeleteExpenseEntryCommand
            {
                Id = id,
                MatterId = matterId,
                UserId = userId
            });

            return result ? NoContent() : NotFound();
        }
    }
}
