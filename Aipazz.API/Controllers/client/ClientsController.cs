using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers.client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] AddClientCommand command)
        {
            var client = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClientByName), new { firstName = client.FirstName, lastName = client.LastName }, client);
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
        public async Task<IActionResult> DeleteClient(string id, [FromQuery] string nic)
        {
            var command = new DeleteClientCommand { id = id, Nic = nic };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetClientByName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var query = new GetClientByNameQuery { FirstName = firstName, LastName = lastName };
            var client = await _mediator.Send(query);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllClientsQuery());
            return Ok(result);
        }

        [HttpGet("{nic}/details")]
        public async Task<IActionResult> GetClientWithDetails(string nic)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _mediator.Send(new GetClientWithDetailsQuery(nic, userId));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("with-entries")]
        public async Task<IActionResult> GetClientsWithEntries()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _mediator.Send(new GetClientsWithEntriesQuery(userId));
            return Ok(result);
        }
    }
}