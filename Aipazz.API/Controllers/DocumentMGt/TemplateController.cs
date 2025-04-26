using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using Aipazz.Application.DocumentMGT.TemplateMgt.Handlers;
using Aipazz.Application.DocumentMGT.TemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.DocumentMGt
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TemplateController(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTemplatesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetTemplateByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Template template)
        {
            await _mediator.Send(new CreateTemplateCommand(template));
            return CreatedAtAction(nameof(GetById), new { id = template.id }, template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Template template)
        {
            if (id != template.id) return BadRequest("ID mismatch");
            await _mediator.Send(new UpdateTemplateCommand(template));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteTemplateCommand(id));
            return NoContent();
        }



    }
}
