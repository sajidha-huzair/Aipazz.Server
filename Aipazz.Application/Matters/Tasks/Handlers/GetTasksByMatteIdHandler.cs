using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.Tasks.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq; // Needed for ToList()


namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class GetTasksByMatterIdQueryHandler : IRequestHandler<GetTasksByMatterIdQuery, List<MatterTask>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksByMatterIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<MatterTask>> Handle(GetTasksByMatterIdQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByMatterIdAsync(request.MatterId ?? string.Empty);
            return tasks.ToList(); // Explicit conversion from IEnumerable to List
        }
    }
}