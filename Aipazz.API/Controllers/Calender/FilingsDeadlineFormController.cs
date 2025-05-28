using Aipazz.Application.Calender.FilingsDeadlineForm.Queries;
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
    }
}