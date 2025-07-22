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
    public class CreateMatterTypeHandler : IRequestHandler<CreateMatterTypeCommand, MatterType>
    {
        private readonly IMatterTypeRepository _repository;

        public CreateMatterTypeHandler(IMatterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<MatterType> Handle(CreateMatterTypeCommand request, CancellationToken cancellationToken)
        {
            var matterType = new MatterType
            {
                id = Guid.NewGuid().ToString(),
                Name = request.Name,
                UserId = request.UserId
            };

            await _repository.AddMatterType(matterType);
            return matterType;
        }
    }
}
