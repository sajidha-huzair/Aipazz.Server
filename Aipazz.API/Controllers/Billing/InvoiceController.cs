using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Invoices.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aipazz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all invoices for the current authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<InvoiceListDto>>> GetInvoicesForCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _mediator.Send(new GetAllInvoicesByUserIdQuery(userId));
            return Ok(result);
        }

        /// <summary>
        /// Get invoice details by ID.
        /// </summary>
        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDetailsDto>> GetInvoiceById(string invoiceId)
        {
            var result = await _mediator.Send(new GetInvoiceDetailsQuery(invoiceId));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
