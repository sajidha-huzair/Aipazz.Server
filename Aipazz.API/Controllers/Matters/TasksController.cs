using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.Matters.Tasks.Commands;
using Aipazz.Application.Matters.Tasks.Queries;

namespace LawfirmAPI.API.Controllers.Matters
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command)
        {
            var task = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTaskByTitle), new { title = task.Title }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var task = await _mediator.Send(command);
            return Ok(task);
        }

        [HttpDelete("{id}/{matterId}")]
        public async Task<IActionResult> DeleteTask(string id, string matterId)
        {
            var command = new DeleteTaskCommand { Id = id, MatterId = matterId };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetTaskByTitle(string title)
        {
            var query = new GetTaskByTitleQuery { Title = title };
            var task = await _mediator.Send(query);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        [HttpGet("Matter/{matterId}")]
        public async Task<IActionResult> GetTasksByMatterId(string matterId)
        {
            var query = new GetTasksByMatterIdQuery { MatterId = matterId };
            var tasks = await _mediator.Send(query);
            return Ok(tasks);
        }
    }
}