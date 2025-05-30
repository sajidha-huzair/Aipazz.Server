using MediatR;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterStatus.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matterStatus.Handlers
{
    public class DeleteStatusHandler : IRequestHandler<DeleteStatusCommand, bool>
    {
        private readonly IStatusRepository _repository;

        public DeleteStatusHandler(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteStatus(request.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
