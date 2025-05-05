using Aipazz.Application.Calender.courtdate.queries;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtDateController : ControllerBase
    {
        private readonly ICourtDateFormRepository _repository;
        private readonly IMediator _mediator;

        public CourtDateController(ICourtDateFormRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourtDates()
        {
            var courtDates = await _mediator.Send(new GetAllCourtDatesQuery());
            return Ok(courtDates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourtDateForm>> GetCourtDate(Guid id)
        {
            var result = await _repository.GetCourtDateFormById(id); // ✅ fixed method name
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}