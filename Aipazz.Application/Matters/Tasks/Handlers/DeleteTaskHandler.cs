using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.Tasks.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.Tasks.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _repository;

        public DeleteTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id!, request.MatterId!);
            return Unit.Value;
        }
    }
}