using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class UpdateTemplateHandler : IRequestHandler<UpdateTemplateCommand, Unit>
    {
        private readonly ITemplateRepository _repository;

        public UpdateTemplateHandler(ITemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            await _repository.UpdateTemplate(request.Template);
            return Unit.Value;
        }
    }
}
