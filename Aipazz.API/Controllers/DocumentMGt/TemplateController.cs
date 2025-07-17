using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using Aipazz.Application.DocumentMGT.TemplateMgt.Handlers;
using Aipazz.Application.DocumentMGT.TemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aipazz.Application.Admin.Interface;

namespace Aipazz.API.Controllers.DocumentMGt
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAdminService _adminService;

        public TemplateController(IMediator mediator, IAdminService adminService)
        {
            _mediator = mediator;
            _adminService = adminService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTemplatesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetTemplateByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Template template)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token.");

            bool isAdmin = _adminService.IsAdminEmail(email);
            if (!isAdmin)
                return Unauthorized("Admin access required.");

            await _mediator.Send(new CreateTemplateCommand(template));
            return CreatedAtAction(nameof(GetById), new { id = template.id }, template);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] Template template)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token.");

            bool isAdmin = _adminService.IsAdminEmail(email);
            if (!isAdmin)
                return Unauthorized("Admin access required.");

            if (id != template.id) return BadRequest("ID mismatch");
            await _mediator.Send(new UpdateTemplateCommand(template));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token.");

            bool isAdmin = _adminService.IsAdminEmail(email);
            if (!isAdmin)
                return Unauthorized("Admin access required.");

            await _mediator.Send(new DeleteTemplateCommand(id));
            return NoContent();
        }
    }
}
