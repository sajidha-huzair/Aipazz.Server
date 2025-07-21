using System.Security.Claims;
using Aipazz.Application.OtherDocuments.Commands;
using Aipazz.Application.OtherDocuments.DTOs;
using Aipazz.Application.OtherDocuments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.OtherDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OtherDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OtherDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
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

            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var documentId = await _mediator.Send(new UploadFileCommand(request, userId, userName));
                
                return Ok(new { 
                    DocumentId = documentId, 
                    Message = "File uploaded successfully", 
                    FileName = request.File.FileName,
                    UploadedBy = userName,
                    MatterId = request.MatterId 
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetOtherDocumentByIdQuery(id, userId));

            if (result == null)
                return NotFound("Document not found.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            try
            {
                var result = await _mediator.Send(new GetAllOtherDocumentsQuery(userId));

                if (result == null || !result.Any())
                    return NotFound("No documents found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching documents: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("matter/{matterId}")]
        public async Task<IActionResult> GetDocumentsByMatterId(string matterId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetOtherDocumentsByMatterIdQuery(matterId, userId));
            return Ok(result);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocument(string id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new DownloadOtherDocumentQuery(id, userId));

            if (result == null)
                return NotFound("Document not found.");

            return File(result.Value.content, result.Value.contentType, result.Value.fileName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _mediator.Send(new DeleteOtherDocumentCommand(id, userId));
            if (!success)
                return NotFound("Document not found or unauthorized.");

            return Ok(new { Message = "Document deleted successfully" });
        }

        [HttpPut("{id}/share-to-team")]
        public async Task<IActionResult> ShareDocumentToTeam(string id, [FromBody] ShareOtherDocumentToTeamDto shareDto)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            await _mediator.Send(new ShareOtherDocumentToTeamCommand(id, shareDto.TeamId, userId));
            return Ok(new { Message = "Document shared to team successfully" });
        }

        [HttpDelete("{id}/remove-from-team")]
        public async Task<IActionResult> RemoveDocumentFromTeam(string id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var success = await _mediator.Send(new RemoveOtherDocumentFromTeamCommand(id, userId));

            if (!success)
                return NotFound("Document not found or not shared with any team.");

            return Ok(new { Message = "Document removed from team successfully" });
        }

        [HttpGet("team-shared")]
        public async Task<IActionResult> GetTeamSharedDocuments()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _mediator.Send(new GetTeamSharedOtherDocumentsQuery(userId));
            return Ok(result);
        }
    }
}