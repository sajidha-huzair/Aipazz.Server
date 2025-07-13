using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class GetTemplateByIdHandler : IRequestHandler<GetTemplateByIdQuery, Template?>
    {
        private readonly ITemplateRepository _repository;

        public GetTemplateByIdHandler(ITemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Template?> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTemplateById(request.Id);
        }
    }
}
