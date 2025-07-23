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
        if (request.PaymentMethod?.ToLower() == "embedded")
        {
            var clientSecret = await _paymentService.CreatePaymentIntentAsync(request);
            return Ok(new StartPaymentResponse { ClientSecret = clientSecret });
        }
        else
        {
            var url = await _paymentService.GeneratePaymentRedirectUrlAsync(request);
            return Ok(new StartPaymentResponse { RedirectUrl = url });
        }
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

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Session;
                    if (session?.Metadata != null && session.AmountTotal.HasValue)
                    {
                        await HandlePaymentAsync(session.Metadata, session.Id, session.AmountTotal.Value);
                    }
                    break;

                case "payment_intent.succeeded":
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    if (intent?.Metadata != null)
                    {
                        await HandlePaymentAsync(intent.Metadata, intent.Id, intent.AmountReceived);
                    }
                    break;

                default:
                    break;
            }


            return Ok();
        }
        catch (StripeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private async Task HandlePaymentAsync(IDictionary<string, string> metadata, string transactionId, long amountCents)
    {
        if (!metadata.TryGetValue("InvoiceId", out var invoiceId)) return;
        metadata.TryGetValue("UserId", out var userId);

        var cmd = new MarkInvoicePaidCommand
        {
            InvoiceId = invoiceId,
            TransactionId = transactionId,
            PaidAmount = (decimal)amountCents / 100,
            UserId = userId ?? string.Empty
        };

        await _mediator.Send(cmd);
    }
}
