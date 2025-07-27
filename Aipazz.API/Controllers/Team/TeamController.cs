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
    [Authorize]
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
            Console.WriteLine("Get All Teams Called");
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            // Extract the email from the claim
            string userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            if (string.IsNullOrWhiteSpace(userEmail))
                return Unauthorized("User email not found in token.");

            var result = await _mediator.Send(new GetAllTeamsQuery(userId, userEmail));
            return Ok(result);
        }

        [HttpGet("{teamId}/documents")]
        [Authorize]
        public async Task<IActionResult> GetTeamDocuments(string teamId)
        {
            Console.WriteLine($"Get Documents by team id called with teamId: {teamId}");
            var result = await _mediator.Send(new GetTeamDocumentsQuery(teamId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(string id)
        {
            Console.WriteLine($"Get Team by id called with id: {id}");
            
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetTeamByIdQuery(id, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            // Extract the owner name from the claim
            string? ownerName = User.Claims
                .FirstOrDefault(c => c.Type == "name")?.Value;
            // Extract the email from the claim
            string userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            if (string.IsNullOrWhiteSpace(ownerName))
                return Unauthorized("User name not found in token.");
            
            var members = command.Members ?? new List<TeamMember>();
            var result = await _mediator.Send(new CreateTeamCommand(command.Name, command.Description, userId, ownerName,userEmail, members));
            
            return Ok(new { 
                Message = "Team created successfully", 
                TeamId = result,
                CreatedBy = ownerName
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] UpdateTeamCommand command)
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            if (id != command.Team.id)
                return BadRequest("ID mismatch");

            command.Team.OwnerId = userId;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            await _mediator.Send(new DeleteTeamCommand(id, userId));
            return NoContent();
        }
    }
}