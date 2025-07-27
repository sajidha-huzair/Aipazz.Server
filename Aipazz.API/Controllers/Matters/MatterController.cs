using Aipazz.Application.Matters.DTO;
using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Domian.Matters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aipazz.API.Controllers.Matters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require user to be authenticated for all actions in this controller
    public class MatterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Helper method to extract the UserId from the JWT token
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found");

        // GET: api/Matter
        // Retrieves all Matters belonging to the authenticated user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetAllMattersQuery(userId));
            return Ok(result);
        }

        // GET: api/Matter/{id}?clientNic=123
        // Retrieves a specific Matter by Id and ClientNic for the authenticated user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, string clientNic)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetMatterByIdQuery(id, clientNic, userId));
            return result == null ? NotFound() : Ok(result);
        }

        // POST: api/Matter
        // Creates a new Matter assigned to the authenticated user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatterCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");

            command.UserId = GetUserId();

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.id, clientNic = command.ClientNic }, result);
        }


        // PUT: api/Matter/{id}
        // Updates an existing Matter that belongs to the authenticated user
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateMatterCommand command)
        {
            if (command == null) return BadRequest("Invalid request.");

            command.Id = id; // ✅ Bind the route param to command
            command.UserId = GetUserId();

            var result = await _mediator.Send(command);
            return Ok(result);
        }


        // DELETE: api/Matter/{id}?clientNic=123
        // Deletes a Matter by Id and ClientNic that belongs to the authenticated user
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string clientNic)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new DeleteMatterCommand
            {
                Id = id,
                ClientNic = clientNic,
                UserId = userId
            });

            return result ? NoContent() : NotFound();
        }

        // GET: api/Matter/titles
        // Retrieves only the titles and Ids of all Matters for the authenticated user
        [HttpGet("titles")]
        public async Task<IActionResult> GetMatterTitles()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetAllMatterTitlesQuery(userId));
            return Ok(result);
        }

        // PUT: api/Matter/{id}/status?clientNic=123
        // Updates the StatusId of a specific Matter for the authenticated user
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            string id,
            [FromQuery] string clientNic,
            [FromBody] UpdateMatterStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewStatusId))
                return BadRequest("New status ID must be provided.");

            var userId = GetUserId();
            await _mediator.Send(new UpdateMatterStatusCommand(id, clientNic, dto.NewStatusId, userId));
            return NoContent();
        }

        // GET: api/Matter/status/{statusId}
        // Retrieves all Matters associated with a specific StatusId for the authenticated user
        [HttpGet("status/{statusId}")]
        public async Task<IActionResult> GetMattersByStatusId(string statusId)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetMattersByStatusIdQuery(statusId, userId));
            return Ok(result);
        }

        // PUT: api/Matter/{id}/share-to-team
        // Shares a Matter with a team
        [HttpPut("{id}/share-to-team")]
        public async Task<IActionResult> ShareMatterToTeam(string id, [FromQuery] string clientNic, [FromBody] ShareMatterToTeamDto shareDto)
        {
            var userId = GetUserId();

            var success = await _mediator.Send(new ShareMatterToTeamCommand(id, clientNic, shareDto.TeamId, userId));

            if (!success)
                return NotFound("Matter not found.");

            return Ok(new { Message = "Matter shared to team successfully" });
        }

        // DELETE: api/Matter/{id}/remove-from-team
        // Removes a Matter from the team
        [HttpDelete("{id}/remove-from-team")]
        public async Task<IActionResult> RemoveMatterFromTeam(string id, [FromQuery] string clientNic)
        {
            var userId = GetUserId();

            var success = await _mediator.Send(new RemoveMatterFromTeamCommand(id, clientNic, userId));

            if (!success)
                return NotFound("Matter not found or not shared with any team.");

            return Ok(new { Message = "Matter removed from team successfully" });
        }

        [HttpGet("{teamid}/team-shared-matters")]
        public async Task<IActionResult> GetTeamSharedMatters(string teamid)
        {
            var result = await _mediator.Send(new GetTeamSharedMattersQuery(teamid));
            return (result == null || !result.Any())
                ? NotFound("No shared matters found for this team.")
                : Ok(result);
        }

        // GET: api/Matter/type-name/{matterTypeName}
        [HttpGet("type-name/{matterTypeName}")]
        public async Task<IActionResult> GetMattersByMatterTypeName(string matterTypeName)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetMattersByMatterTypeIdQuery(matterTypeName, userId));
            return Ok(result);
        }

        // GET: api/Matter/client/{clientNic}
        [HttpGet("getmattersbyclientnic/{clientNic}")]
        public async Task<IActionResult> GetMattersByNic(string clientNic)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetMattersByClientNicQuery(clientNic, userId));
            return Ok(result);


        }

        // GET: api/Matter/summary
        [HttpGet("{id}/updates")]
        public async Task<ActionResult<List<MatterUpdateHistory>>> GetMatterUpdates(
    string id,
    [FromQuery] string clientNic,
    [FromQuery] string userId)
        {
            try
            {
                var updates = await _mediator.Send(new GetMatterUpdatesQuery
                {
                    MatterId = id,
                    ClientNic = clientNic,
                    UserId = userId
                });

                return Ok(updates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
