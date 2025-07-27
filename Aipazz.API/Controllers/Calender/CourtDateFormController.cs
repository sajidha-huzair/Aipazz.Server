using Aipazz.Application.Calendar.CourtDateForms.Queries;
using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.CourtDateForms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calender
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateFormController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            string? UserId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("UserId: " + UserId);
            
            var result = await mediator.Send(new GetCourtDateFormListQuery(UserId));
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetCourtDateFormByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCourtDateFormCommand command)
        {
            string? UserId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            Console.WriteLine("UserId: " + UserId);
            command.UserId = UserId;
            var result = await mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCourtDateForm([FromRoute]Guid id, [FromBody]UpdateCourtDateFormCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL and body do not match.");

            command.Id = id;
            var result = await mediator.Send(command);
            if (result == null) return NotFound();

            return Ok(result);
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteCourtDateFormCommand(id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}