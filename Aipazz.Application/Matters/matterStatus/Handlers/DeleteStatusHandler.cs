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
            // Fetch status by Id and UserId to verify ownership
            var status = await _repository.GetStatusById(request.Id, request.UserId);
            if (status == null)
            {
                return false; // Not found or not authorized
            }

            try
            {
                await _repository.DeleteStatus(request.Id, request.UserId); // Secure delete
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
