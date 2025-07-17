using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
{
    public class GetAllTemplatesHandler : IRequestHandler<GetAllTemplatesQuery, List<Template>>
    {
        private readonly ITemplateRepository _repository;

        public GetAllTemplatesHandler(ITemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Template>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllTemplates();
        }
    }
}
