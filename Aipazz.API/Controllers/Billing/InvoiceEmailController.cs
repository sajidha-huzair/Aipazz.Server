using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Billing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using MediatR;
using System.Security.Claims;
using Aipazz.Application.Common.Aipazz.Application.Common;

namespace Aipazz.API.Controllers.Billing
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceEmailController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;
        public InvoiceEmailController(IMediator mediator, IUserContext userContext)
        {
                _mediator = mediator;
               _userContext = userContext;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found in token claims");

        // POST: api/InvoiceEmail/send-invoice-link
        [HttpPost("send-invoice-link")]
        public async Task<IActionResult> SendInvoiceLink([FromBody] SendInvoiceLinkRequest request)
        {
            var userId = GetUserId();
            var senderEmail = _userContext.Email;

            var result = await _mediator.Send(
                new SendInvoiceLinkCommand(userId, request.InvoiceId, request.RecipientEmail,senderEmail));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = "Invoice link sent successfully." });
        }

        [HttpPost("verify-token")]
        [AllowAnonymous] // so client without login can access this
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenRequest request)
        {
            var result = await _mediator.Send(new VerifyTokenCommand(request.Token));

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Token verified", invoiceId = result.InvoiceId });
        }


    }

}
