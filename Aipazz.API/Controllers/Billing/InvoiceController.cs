using Aipazz.Application.Billing.DTOs;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Billing.Invoices.Queries;
using Aipazz.Application.Common.Aipazz.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aipazz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // remove if your API is public
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IExpenseEntryRepository _expenseRepo;
        private readonly IInvoicePdfGenerator _pdfGenerator;
        private readonly IInvoiceRepository _invoiceRepo;

        public InvoicesController(IMediator mediator,
        ITimeEntryRepository timeRepo,
        IExpenseEntryRepository expenseRepo,
        IInvoicePdfGenerator pdfGenerator,
        IInvoiceRepository invoiceRepo)
        {
            _mediator = mediator;
            _timeRepo = timeRepo;
            _expenseRepo = expenseRepo;
            _pdfGenerator = pdfGenerator;
            _invoiceRepo = invoiceRepo;
        }

        // ─────────────────────── Helpers ───────────────────────
        private string GetUserId() =>
            User.FindFirstValue("oid")        // Azure AD (object id)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ApplicationException("UserId claim missing");

        // ─────────────────────── CREATE ───────────────────────
        /// <summary>
        /// Generate a new invoice from selected entry IDs.
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult<string>> GenerateInvoice([FromBody] GenerateInvoiceDto dto)
        {
            var id = await _mediator.Send(new GenerateInvoiceFromSelectionCommand
            {
                UserId = GetUserId(),
                ClientNic = dto.ClientNic,
                EntryIds = dto.EntryIds,
                Force = dto.Force      // ← propagate the flag
            });

            return CreatedAtAction(nameof(GetInvoiceById), new { id }, id);
        }


        public record GenerateInvoiceDto(string ClientNic, List<string> EntryIds, bool Force = false);

        // ─────────────────────── GET LIST ───────────────────────
        [HttpGet]
        public async Task<ActionResult<List<InvoiceListDto>>> GetAllForUser()
        {
            var list = await _mediator.Send(
                new GetAllInvoicesForUserQuery(GetUserId()));
            return Ok(list);
        }

        // ─────────────────────── GET SINGLE ───────────────────────
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDetailsDto>> GetInvoiceById(string id)
        {
            var item = await _mediator.Send(
                new GetInvoiceByIdQuery(id, GetUserId()));

            return item is null ? NotFound() : Ok(item);
        }

        // ─────────────────────── UPDATE ───────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(string id,
            [FromBody] InvoiceDetailsDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            dto.UpdatedBy = GetUserId();
            await _mediator.Send(new UpdateInvoiceCommand(dto));
            return NoContent();
        }

        // ─────────────────────── DELETE ───────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            await _mediator.Send(new DeleteInvoiceCommand(id, GetUserId()));
            return NoContent();
        }

        [HttpGet("unbilled-tree")]
        public async Task<ActionResult<List<ClientWithMattersDto>>> GetUnbilledTree()
        {
            var tree = await _mediator.Send(
            new GetClientsWithUnbilledEntriesQuery(GetUserId()));
            return Ok(tree);
        }

        [HttpGet("entries/billed-tree")]
        public async Task<ActionResult<List<ClientWithMattersDto>>> GetBilled() =>
        Ok(await _mediator.Send(new GetClientsWithBilledEntriesQuery(GetUserId())));


        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GenerateInvoicePdf(string id)
        {
            var userId = GetUserId();
            var invoice = await _invoiceRepo.GetByIdAsync(id, userId);  // ← domain object
            if (invoice == null) return NotFound();

            var timeEntries = await _timeRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, userId);
            var expenseEntries = await _expenseRepo.GetAllEntriesByIdsAsync(invoice.EntryIds, userId);

            var pdfBytes = await _pdfGenerator.GeneratePdfAsync(invoice, timeEntries, expenseEntries);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }

        [HttpPut("{id}/notes")]
        public async Task<ActionResult<string>> UpdateNotes(string id, [FromBody] NotesDto dto)
        {
            var url = await _mediator.Send(new UpdateInvoiceNotesCommand(
                id,
                GetUserId(),
                dto.FooterNotes,
                dto.PaymentProfileNotes));

            return Ok(url);        // returns fresh PdfFileUrl
        }

        public record NotesDto(string FooterNotes, string PaymentProfileNotes);

    }
}
