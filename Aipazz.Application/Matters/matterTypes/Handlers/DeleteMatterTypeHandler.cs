using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterTypes.Commands;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Handlers
{
    public class DeleteMatterTypeHandler : IRequestHandler<DeleteMatterTypeCommand, bool>
    {
        private readonly IMatterTypeRepository _repository;

        public DeleteMatterTypeHandler(IMatterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteMatterTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetMatterTypeById(request.Id, request.UserId);
            if (existing == null) return false;

            await _repository.DeleteMatterType(request.Id, request.UserId);
            return true;
        }
    }
}
