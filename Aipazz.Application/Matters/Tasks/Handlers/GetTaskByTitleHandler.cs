using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.Tasks.Queries;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class GetTaskByTitleQueryHandler : IRequestHandler<GetTaskByTitleQuery,MatterTask?>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByTitleQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<MatterTask?> Handle(GetTaskByTitleQuery request, CancellationToken cancellationToken)
        {
            return await _taskRepository.GetByTitleAsync(request.Title ?? string.Empty);
        }
    }
}