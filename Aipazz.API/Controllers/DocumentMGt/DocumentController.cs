
using System.Security.Claims;
using Aipazz.Application.DocumentMgt.Queries;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.DTO;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenXmlPowerTools;

namespace Aipazz.API.Controllers.DocumentMGt
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public DocumentController(IMediator mediator)
        {
            _mediatR = mediator;

        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateWord([FromBody] HtmlInput input)
        {
            var fileBytes = await _mediatR.Send(new GenerateWordFromHtmlQuery(input.Html));
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Generated.docx");
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveDocument([FromBody] SaveDocumentCommand command)
        {
            var savedFileName = await _mediatR.Send(command);
            return Ok(new { Message = "Document saved successfully", FileName = savedFileName });
        }

        [HttpPost("SaveWithUserid")]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request)
        {
            var id = await _mediatR.Send(new CreateDocumentCommand(request));
            return Ok(new { DocumentId = id });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDocumentRequest request)
        {
            if (id != request.DocumentId) return BadRequest("ID mismatch.");
            var result = await _mediatR.Send(new UpdateDocumentCommand(request));
            return result ? Ok("Updated") : NotFound("Document not found");
        }


        [HttpGet("{id}")]
     
        public async Task<IActionResult> GetDocumentById(string id)
        {
            
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;


            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediatR.Send(new GetDocumentByIdQuery(id, userId));

            if (result == null)
                return NotFound("Document not found.");

            return Ok(result);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDocuments()
        {
            // Extract the user ID from the claim
            string userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            // If user ID is not found, return unauthorized
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            try
            {
                // Send query to Mediator to fetch documents based on userId
                var result = await _mediatR.Send(new GetAllDocumentsQuery(userId));

                // If no documents found, return an empty response or not found
                if (result == null || !result.Any())
                {
                    return NotFound("No documents found.");
                }

                // Return OK with the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _mediatR.Send(new DeleteDocumentCommand(id, userId));
            if (!success)
                return NotFound("Document not found or unauthorized.");

            return NoContent();
        }











    }
}
    
