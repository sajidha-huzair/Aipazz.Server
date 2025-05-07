using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
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
            var matter = await _repository.GetMatterById(request.Id, request.Title);
            if (matter == null)
            {
                throw new KeyNotFoundException($"Matter with Id {request.Id} and Title {request.Title} not found.");
            }

            await _repository.DeleteMatter(request.Id, request.Title);
            return true;
        }
    }
}
