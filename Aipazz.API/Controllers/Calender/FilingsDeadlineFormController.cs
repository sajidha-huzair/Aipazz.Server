using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.FilingsDeadlineForm.Queries;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilingsDeadlineFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilingsDeadlineFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllFilingsDeadlineFormsQuery());
            return Ok(result);
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetFilingsDeadlineFormByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddFilingsDeadlineFormCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateFilingsDeadlineFormCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);

            if (result == null) return NotFound();
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteFilingsDeadlineFormCommand(id));

            if (!result) return NotFound();
            return NoContent(); // 204 No Content
        }




    }
}