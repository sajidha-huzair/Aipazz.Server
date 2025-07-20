using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Tasks.Queries;
using Aipazz.Application.Matters.Interfaces;
using System.Linq; // Needed for ToList()

namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, List<MatterTask>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<MatterTask>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.ToList(); // Fixes the type mismatch
        }
    }
}