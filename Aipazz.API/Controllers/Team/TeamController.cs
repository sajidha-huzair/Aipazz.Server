using System.Security.Claims;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Queries;
using Aipazz.Domian.Team;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Team
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            var result = await _mediator.Send(new GetAllTeamsQuery(userId));
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(string id)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            var result = await _mediator.Send(new GetTeamByIdQuery(id, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            var userId = "test-user-id";
            
            // Pass userId to the command
            var members = command.Members ?? new List<TeamMember>();
            var result = await _mediator.Send(new CreateTeamCommand(command.Name, command.Description, userId, members));
            return Ok(new { Message = "Team created successfully", TeamId = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] UpdateTeamCommand command)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            if (id != command.Team.id)
                return BadRequest("ID mismatch");

            command.Team.OwnerId = userId;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            await _mediator.Send(new DeleteTeamCommand(id, userId));
            return NoContent();
        }
    }
}