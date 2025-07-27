using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aipazz.Application.Matters.Tasks.Commands;
using Aipazz.Application.Matters.Tasks.Queries;

namespace LawfirmAPI.API.Controllers.Matters
{
    [ApiController]
    [Route("api/[controller]")]
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
            try
            {
                var task = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetTaskByTitle), new { title = task.Title }, task);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create task: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                var task = await _mediator.Send(command);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update task: {ex.Message}");
            }
        }

        [HttpDelete("{id}/{matterId}")]
        public async Task<IActionResult> DeleteTask(string id, string matterId)
        {
            try
            {
                var command = new DeleteTaskCommand { Id = id, MatterId = matterId };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete task: {ex.Message}");
            }
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetTaskByTitle(string title)
        {
            try
            {
                var query = new GetTaskByTitleQuery { Title = title };
                var task = await _mediator.Send(query);
                if (task == null)
                    return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get task: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetAllTasksQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get tasks: {ex.Message}");
            }
        }

        [HttpGet("Matter/{matterId}")]
        public async Task<IActionResult> GetTasksByMatterId(string matterId)
        {
            try
            {
                var query = new GetTasksByMatterIdQuery { MatterId = matterId };
                var tasks = await _mediator.Send(query);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get tasks for matter: {ex.Message}");
            }
        }
    }
}
