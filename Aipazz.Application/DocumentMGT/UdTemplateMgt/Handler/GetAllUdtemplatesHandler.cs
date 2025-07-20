using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Handler
{
    public class GetAllUdtemplatesHandler : IRequestHandler<GetAllUdtemplatesQuery, List<Udtemplate>>
    {
        private readonly IUdtemplateRepository _repository;

        public GetAllUdtemplatesHandler(IUdtemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Udtemplate>> Handle(GetAllUdtemplatesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllTemplatesByUserId(request.UserId);
        }
    }
}