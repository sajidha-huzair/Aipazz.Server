using Aipazz.Application.Calender.TeamMeetingForms.Queries;
using Aipazz.Domian.Calender;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calendar
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamMeetingFormController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamMeetingFormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamMeetingForm>>> GetAll()
        {
            var query = new GetAllTeamMeetingFormsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Other endpoints will go here later
    }
}