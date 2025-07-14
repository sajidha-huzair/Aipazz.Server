using System.Security.Claims;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.DTOs;
using Aipazz.Application.Team.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Team
{
    [Route("api/teams/{teamId}/members")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamMembers(string teamId)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            var result = await _mediator.Send(new GetTeamMembersQuery(teamId, userId));
            return Ok(result);
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetTeamMemberById(string teamId, string memberId)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            var result = await _mediator.Send(new GetTeamMemberByIdQuery(teamId, memberId, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeamMember(string teamId, [FromBody] AddTeamMemberDto memberDto)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            var result = await _mediator.Send(new AddTeamMemberCommand(teamId, userId, memberDto));
            return CreatedAtAction(nameof(GetTeamMemberById), new { teamId, memberId = result.Id }, result);
        }

        [HttpPut("{memberId}")]
        public async Task<IActionResult> UpdateTeamMember(string teamId, string memberId, [FromBody] UpdateTeamMemberDto memberDto)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            await _mediator.Send(new UpdateTeamMemberCommand(teamId, memberId, userId, memberDto));
            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<IActionResult> DeleteTeamMember(string teamId, string memberId)
        {
            // Using a test user ID for now
            var userId = "test-user-id";

            await _mediator.Send(new DeleteTeamMemberCommand(teamId, memberId, userId));
            return NoContent();
        }
    }
}