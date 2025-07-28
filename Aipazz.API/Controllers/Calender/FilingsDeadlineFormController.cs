using Aipazz.Application.Calender.Commands.FilingsDeadlineForms;
using Aipazz.Application.Calender.FilingsDeadlineForm.Queries;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string? userId = User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    ?.Value;
                Console.WriteLine("User Id :" + userId);
                var result = await _mediator.Send(new GetAllFilingsDeadlineFormsQuery(userId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                var result = await _mediator.Send(new GetFilingsDeadlineFormByIdQuery(id));
                if (result == null)
                    return NotFound($"Filing deadline with ID {id} not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddFilingsDeadlineFormCommand command)
        {
            try
            {
                // Basic validation
                if (command == null)
                    return BadRequest("Command cannot be null");
                    
                if (string.IsNullOrWhiteSpace(command.Title))
                    return BadRequest("Title is required");
                    
                
                    
                if (command.Date == default(DateTime))
                    return BadRequest("Valid date is required");
                
                string? userId = User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    ?.Value;
                Console.WriteLine("userId: " + userId);
                command.UserId = userId;

                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateFilingsDeadlineFormCommand command)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                if (command == null)
                    return BadRequest("Command cannot be null");
                    
                if (id != command.Id) 
                    return BadRequest("ID mismatch between URL and request body");
                    
                // Basic validation
                if (string.IsNullOrWhiteSpace(command.Title))
                    return BadRequest("Title is required");
                    
                if (string.IsNullOrWhiteSpace(command.AssignedMatter))
                    return BadRequest("AssignedMatter is required");
                    
                if (command.Date == default(DateTime))
                    return BadRequest("Valid date is required");

                var result = await _mediator.Send(command);

                if (result == null) 
                    return NotFound($"Filing deadline with ID {id} not found");
                    
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID provided");
                    
                var result = await _mediator.Send(new DeleteFilingsDeadlineFormCommand(id));

                if (!result) 
                    return NotFound($"Filing deadline with ID {id} not found");
                    
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}