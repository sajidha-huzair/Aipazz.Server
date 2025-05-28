using Aipazz.Application.Calender.FilingsDeadlineForm.Queries;
using Aipazz.Application.Calender.Queries.FilingsDeadlineForms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilingsDeadlineFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilingsDeadlineFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllFilingsDeadlineFormsQuery());
            return Ok(result);
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetFilingsDeadlineFormByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}