using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand,Unit>
    {
        private readonly ITemplateRepository _repository;
        public DeleteTemplateHandler(ITemplateRepository repository)
        {
            _repository = repository;

        }

        public async Task<Unit> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteTemplate(request.Id);
            return Unit.Value;
        }
    }
}
