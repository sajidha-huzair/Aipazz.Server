using Aipazz.Application.Matters.Tasks.Commands;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, MatterTask>
    {
        private readonly ITaskRepository _taskRepository;

        public AddTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<Aipazz.Domian.Matters.MatterTask> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new MatterTask
            {
                id = Guid.NewGuid().ToString(),
                MatterId = request.MatterId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = request.Status,
                CreatedDate = request.CreatedDate ?? DateTime.UtcNow,
                AssignedTo = request.AssignedTo
            };

            await _taskRepository.CreateAsync(task);
            return task;
        }
    }
}