using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
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
        private readonly IPaymentService _paymentService;

        public PaymentController(IMediator mediator, IPaymentService paymentService)
        {
            _mediator = mediator;
            _paymentService = paymentService;
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

        [HttpPost("start")]
        public async Task<ActionResult<StartPaymentResponse>> StartPayment([FromBody] StartPaymentRequest request)
        {
            var url = await _paymentService.GeneratePaymentRedirectUrlAsync(request);
            return Ok(new StartPaymentResponse { RedirectUrl = url });
        }
    }

}
