using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.client.Commands;
using Aipazz.Application.client.Queries;
using Aipazz.Application.client.DTOs;
using System.Security.Claims;

namespace Aipazz.API.Controllers.client
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator) => _mediator = mediator;

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found");

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetAllClientsQuery(userId));
            return Ok(result);
        }

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

        // Fixed team sharing endpoints
        [HttpPut("{clientNic}/share-to-team")]
        [Authorize]
        public async Task<IActionResult> ShareClientToTeam(string clientNic, [FromBody] ShareClientToTeamDto shareDto)
        {
            var userId = GetUserId();
            await _mediator.Send(new ShareClientToTeamCommand(clientNic, shareDto.TeamId, userId));
            return Ok(new { Message = "Client shared to team successfully" });
        }

        [HttpPut("{clientNic}/remove-from-team")]
        [Authorize]
        public async Task<IActionResult> RemoveClientFromTeam(string clientNic)
        {
            var userId = GetUserId();
            var success = await _mediator.Send(new RemoveClientFromTeamCommand(clientNic, userId));
            
            if (!success)
                return NotFound("Client not found or not shared with any team.");

            return Ok(new { Message = "Client removed from team successfully" });
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
       
        [HttpGet("check-nic/{nic}")]
        public async Task<IActionResult> CheckNICExists(string nic)
        {
            var userId = GetUserId();
            var exists = await _mediator.Send(new CheckClientNICExistsQuery(nic, userId));
            return Ok(exists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string nic)
        {
            var result = await _mediator.Send(new GetClientByIdQuery(id, nic, GetUserId()));
            return result == null ? NotFound() : Ok(result);
        }

        // GET api/clients/with-entries
        [HttpGet("with-entries")]
        public async Task<IActionResult> GetClientsWithEntries()
        {
            var list = await _mediator.Send(new GetClientsWithEntriesQuery(GetUserId()));
            return Ok(list);      // 200 + full client‑matter‑entry tree
        }

    }
}