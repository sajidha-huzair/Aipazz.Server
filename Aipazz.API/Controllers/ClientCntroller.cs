using Aipazz.Application.Commands;
using Aipazz.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
        {
            var clientId = await _mediator.Send(command);
            return Ok(clientId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            await _mediator.Send(new DeleteClientCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(string id)
        {
            var client = await _mediator.Send(new GetClientByIdQuery { Id = id });
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClients([FromQuery] string searchTerm)
        {
            var clients = await _mediator.Send(new GetClientsBySearchTermQuery { SearchTerm = searchTerm });
            return Ok(clients);
        }
    }
}