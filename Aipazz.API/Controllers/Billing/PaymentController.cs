using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _config;
    private readonly IMediator _mediator;

    public PaymentController(IPaymentService paymentService, IConfiguration config, IMediator mediator)
    {
        _paymentService = paymentService;
        _config = config;
        _mediator = mediator;
    }

    [HttpPost("start")]
    public async Task<ActionResult<StartPaymentResponse>> StartPayment([FromBody] StartPaymentRequest request)
    {
        var url = await _paymentService.GeneratePaymentRedirectUrlAsync(request);
        return Ok(new StartPaymentResponse { RedirectUrl = url });
    }

    [HttpPost("stripe-webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var secret = _config["Stripe:WebhookSecret"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                secret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                var invoiceId = session.Metadata["InvoiceId"];
                var userId = session.Metadata["UserId"];

                var cmd = new MarkInvoicePaidCommand
                {
                    InvoiceId = invoiceId,
                    TransactionId = session.Id,
                    PaidAmount = (decimal)session.AmountTotal / 100,
                    UserId = userId
                };

                await _mediator.Send(cmd);
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
