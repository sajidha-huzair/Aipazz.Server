using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class DeleteMatterCommandHandler : IRequestHandler<DeleteMatterCommand, bool>
    {
        private readonly IMatterRepository _repository;

        public DeleteMatterCommandHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteMatterCommand request, CancellationToken cancellationToken)
        {
            var matter = await _repository.GetMatterById(request.Id, request.ClientNic, request.UserId);
            if (matter == null)
            {
                throw new KeyNotFoundException($"Matter with Id {request.Id} and ClientNic {request.ClientNic} not found or unauthorized.");
            }

            await _repository.DeleteMatter(request.Id, request.ClientNic, request.UserId);
            return true;
        }
    }
}
