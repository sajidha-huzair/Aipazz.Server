using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Application.Matters.matterTypes.Commands;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Handlers
{
    public class UpdateMatterTypeHandler : IRequestHandler<UpdateMatterTypeCommand, MatterType>
    {
        private readonly IMatterTypeRepository _repository;

        public UpdateMatterTypeHandler(IMatterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<MatterType> Handle(UpdateMatterTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetMatterTypeById(request.Id, request.UserId);
            if (existing == null) throw new KeyNotFoundException("Matter type not found.");

            existing.Name = request.Name;
            await _repository.UpdateMatterType(existing);

            return existing;
        }
    }
}
