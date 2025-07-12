using Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands;
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
    }
}
