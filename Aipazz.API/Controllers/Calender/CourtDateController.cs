
using Aipazz.Application.Calender.courtdate.queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourtDateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourtDates()
        {
            var courtDates = await _mediator.Send(new GetAllCourtDatesQuery());
            return Ok(courtDates);
        }
    }
    
    
    
    
}