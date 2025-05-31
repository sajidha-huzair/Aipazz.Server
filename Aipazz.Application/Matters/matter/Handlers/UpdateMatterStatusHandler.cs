using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Matters.matter.Commands;
using Aipazz.Application.Matters.Interfaces;
using MediatR;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class UpdateMatterStatusHandler : IRequestHandler<UpdateMatterStatusCommand, Unit>
    {
        private readonly IMatterRepository _repository;

        public UpdateMatterStatusHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateMatterStatusCommand request, CancellationToken cancellationToken)
        {
            var matter = await _repository.GetMatterById(request.MatterId, request.ClientNIC);
            if (matter == null)
                throw new KeyNotFoundException("Matter not found");

            matter.StatusId = request.NewStatusId;

            await _repository.UpdateMatter(matter);
            return Unit.Value;
        }
    }
}

