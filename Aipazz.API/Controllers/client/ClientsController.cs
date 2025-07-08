using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Queries;
<<<<<<< Updated upstream
using System.Threading.Tasks;
using Aipazz.Application.Billing.TimeEntries.Queries;
=======
using System.Security.Claims;
>>>>>>> Stashed changes

namespace Aipazz.API.Controllers.client
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator) => _mediator = mediator;

<<<<<<< Updated upstream
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] AddClientCommand command)
        {
            var client = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClientByName), new { client.name }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(string id, [FromBody] UpdateClientCommand command)
        {
            if (id != command.id)
                return BadRequest("ID mismatch");

            var client = await _mediator.Send(command);
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            var command = new DeleteClientCommand { id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetClientByName(string name)
        {
            var query = new GetClientByNameQuery { Name = name };
            var client = await _mediator.Send(query);
            if (client == null)
                return NotFound();
            return Ok(client);
        }
=======
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found");
>>>>>>> Stashed changes

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetAllClientsQuery(userId));
            return Ok(result);
        }
<<<<<<< Updated upstream
=======

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddClientCommand command)
        {
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.id, nic = result.nic },
                result
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] UpdateClientCommand command)
        {
            if (id != command.id)
                return BadRequest("ID mismatch");

            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] string nic)
        {
            await _mediator.Send(new DeleteClientCommand
            {
                id = id,
                nic = nic,
                UserId = GetUserId()
            });
            return NoContent();
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName(
            [FromQuery] string firstName,
            [FromQuery] string lastName)
        {
            var result = await _mediator.Send(new GetClientByNameQuery(
                firstName ?? string.Empty,
                lastName ?? string.Empty,
                GetUserId()
            ));
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string nic)
        {
            var result = await _mediator.Send(new GetClientByIdQuery(id, nic, GetUserId()));
            return result == null ? NotFound() : Ok(result);
        }
>>>>>>> Stashed changes
    }
}