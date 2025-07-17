using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.Tasks.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Aipazz.Domian.Matters.MatterTask>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<Aipazz.Domian.Matters.MatterTask> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Aipazz.Domian.Matters.MatterTask
            {
                id = request.Id,
                MatterId = request.MatterId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = request.Status
            };
            await _taskRepository.UpdateAsync(task);
            return task;
        }
    }
}