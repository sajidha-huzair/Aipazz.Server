using Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aipazz.API.Controllers.DocumentMGt
{
    [Route("api/[controller]")]
    [ApiController]
    public class UdTemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UdTemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetAllUdtemplatesQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetUdtemplateByIdQuery(id, userId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUdtemplateCommand command)
        {
            if (command == null)
            {
                return BadRequest("Command cannot be null.");
            }

            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            // Set the user ID from the token to the template
            command.Udtemplate.Userid = userId;

            var result = await _mediator.Send(command);
            return Ok(new { Message = "Template created successfully", TemplateId = result });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUdtemplateCommand command)
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            if (id != command.Udtemplate.id)
                return BadRequest("ID mismatch");

            // Set the user ID from the token to the template
            command.Udtemplate.Userid = userId;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            await _mediator.Send(new DeleteUdtemplateCommand(id, userId));
            return NoContent();
        }
    }
}
