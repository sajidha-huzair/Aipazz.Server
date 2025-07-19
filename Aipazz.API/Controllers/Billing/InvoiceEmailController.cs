using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Billing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Aipazz.Application.Common;
using Aipazz.Application.Common.Aipazz.Application.Common;

namespace Aipazz.API.Controllers.Billing
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceEmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;

        public InvoiceEmailController(IMediator mediator, IUserContext userContext)
        {
            _mediator = mediator;
            _userContext = userContext;
        }

        // ✅ Auth required: Lawyer triggers email
        [HttpPost("send-invoice-link")]
        [Authorize]
        public async Task<IActionResult> SendInvoiceLink([FromBody] SendInvoiceLinkRequest request)
        {
            var userId = _userContext.UserId;
            var senderEmail = _userContext.Email;

            var result = await _mediator.Send(
                new SendInvoiceLinkCommand(userId, request.InvoiceId, request.RecipientEmail, senderEmail));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = "Invoice link sent successfully." });
        }

        [HttpPost("generate-link")]
        [Authorize]
        public async Task<IActionResult> GenerateLink([FromBody] GenerateLinkRequest request)
        {
            var userId = _userContext.UserId;

            var link = await _mediator.Send(new GenerateInvoiceAccessLinkCommand(userId, request.InvoiceId));

            return Ok(new { link });
        }


        // ❌ No auth: Client accesses link
        [HttpPost("verify-token")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenRequest request)
        {
            var result = await _mediator.Send(new VerifyTokenCommand(request.Token));

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Token verified", invoiceId = result.InvoiceId });
        }

        // ❌ No auth: Client enters OTP
        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyInvoiceLink([FromBody] VerifyInvoiceLinkRequest request)
        {
            var result = await _mediator.Send(new VerifyInvoiceLinkCommand
            {
                Token = request.Token,
                Otp = request.Otp
            });

            if (!result.IsValid)
                return BadRequest(result);

            return Ok(result);
        }

        // ❌ No auth: Client wants OTP resent
        [HttpPost("send-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest("Invalid or expired token.");

            return Ok("OTP has been sent to the recipient email.");
        }

        // ❌ No auth: Client views PDF → token marked used
        [HttpPost("update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateToken([FromBody] UpdateTokenCommand command)
        {
            var success = await _mediator.Send(command);

            if (!success)
                return BadRequest("Token not found or update failed.");

            return Ok("Token updated successfully.");
        }
    }
}
