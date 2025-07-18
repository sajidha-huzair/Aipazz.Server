using System.Security.Claims;
using Aipazz.Application.DocumentMgt.Queries;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.DTO;
using Aipazz.Application.DocumentMGT.DTOs;
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
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            // Extract the user name from the claim
            string? userName = User.Claims
                .FirstOrDefault(c => c.Type == "name")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized("User name not found in token.");

            // Set the user ID and name from the token to the request
            request.UserId = userId;
            request.UserName = userName;
            // MatterId is already set by the client in the request

            var id = await _mediatR.Send(new CreateDocumentCommand(request));
            
            return Ok(new { 
                DocumentId = id, 
                Message = "Document created successfully", 
                CreatedBy = userName,
                MatterId = request.MatterId 
            });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDocumentRequest request)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            // Set the route ID to the DocumentId in the request
            // This ensures they always match
            request.DocumentId = id;

            // Set the user ID from the token to the request
            request.UserId = userId;

            var result = await _mediatR.Send(new UpdateDocumentCommand(request));
            return result ? Ok("Updated") : NotFound("Document not found");
        }

        [HttpPut("{id}/share-to-team")]
        [Authorize]
        public async Task<IActionResult> ShareDocumentToTeam(string id, [FromBody] ShareDocumentToTeamDto shareDto)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            await _mediatR.Send(new ShareDocumentToTeamCommand(id, shareDto.TeamId, userId));
            return Ok(new { Message = "Document shared to team successfully" });
        }

        [HttpGet("team-shared")]
        [Authorize]
        public async Task<IActionResult> GetTeamSharedDocuments()
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediatR.Send(new GetTeamSharedDocumentsQuery(userId));
            return Ok(result);
        }

        [HttpDelete("{id}/remove-from-team")]
        [Authorize]
        public async Task<IActionResult> RemoveDocumentFromTeam(string id)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var success = await _mediatR.Send(new RemoveDocumentFromTeamCommand(id, userId));

            if (!success)
                return NotFound("Document not found or not shared with any team.");

            return Ok(new { Message = "Document removed from team successfully" });
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
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
            string? userId = User.Claims
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
                // Log the error properly
                Console.WriteLine($"Error fetching documents: {ex.Message}");
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

        [HttpGet("matter/{matterId}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentsByMatterId(string matterId)
        {
            // Extract the user ID from the claim
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediatR.Send(new GetDocumentsByMatterIdQuery(matterId, userId));
            return Ok(result);
        }
    }
}
