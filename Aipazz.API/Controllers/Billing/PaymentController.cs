using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Invoices.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Billing
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("payhere-notify")]
        public async Task<IActionResult> Notify([FromForm] PayHereCallbackDto data)
        {
            if (data.status == "2")
            {
                var cmd = new MarkInvoicePaidCommand
                {
                    InvoiceId = data.order_id,
                    TransactionId = data.payment_id,
                    PaidAmount = data.paid_amount,
                    UserId = data.custom_1 // you can pass this from frontend
                };

                await _mediator.Send(cmd);
            }

            return Ok(); // PayHere requires this
        }
    }

}
