using Aipazz.Application.Calendar.CourtDateForms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Domian.Calendar;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourtDateFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourtDateForm>>> GetAll()
        {
            var result = await _mediator.Send(new GetCourtDateFormListQuery());
            return Ok(result);
        }
    }
}