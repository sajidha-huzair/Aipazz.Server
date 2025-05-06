using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Queries;
using System.Threading.Tasks;
using Aipazz.Application.Billing.TimeEntries.Queries;

namespace LawfirmAPI.API.Controllers
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
            return CreatedAtAction(nameof(GetClientByName), new { name = client.name }, client);
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllClientsQuery());
            return Ok(result);
        }
    }
}