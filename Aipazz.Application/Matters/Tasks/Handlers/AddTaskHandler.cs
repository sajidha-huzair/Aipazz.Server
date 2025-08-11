using Aipazz.Application.Matters.Tasks.Commands;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Billing.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, MatterTask>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;

        public AddTaskCommandHandler(ITaskRepository taskRepository, IEmailService emailService)
        {
            _taskRepository = taskRepository;
            _emailService = emailService;
        }

        public async Task<MatterTask> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new MatterTask
            {
                id = Guid.NewGuid().ToString(),
                MatterId = request.MatterId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = request.Status ?? "Pending",
                CreatedDate = request.CreatedDate ?? DateTime.UtcNow,
                AssignedTo = request.AssignedTo
            };

            await _taskRepository.CreateAsync(task);

            // Send email notification if AssignedTo has a value
            if (!string.IsNullOrWhiteSpace(task.AssignedTo))
            {
                var htmlBody = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #2b7a78;'>You have been assigned a new task!</h2>
                        <p><strong>Task:</strong> {task.Title}</p>
                        <p><strong>Description:</strong> {task.Description}</p>
                        <p><strong>Due Date:</strong> {task.DueDate?.ToString("dd MMM yyyy")}</p>
                        <p>Please log in to the Aipazz dashboard to view more details.</p>
                        <a href='https://witty-field-0e9483e0f.6.azurestaticapps.net/' 
                           style='display:inline-block;background-color:#2b7a78;color:#fff;padding:12px 20px;text-decoration:none;border-radius:6px;margin-top:10px;'>
                           Go to Dashboard
                        </a>
                    </body>
                </html>";

                await _emailService.SendOtpEmailAsync(
                    task.AssignedTo,
                    $"New Task Assigned: {task.Title}",
                    htmlBody
                );
            }

            return task;
        }
    }
}
