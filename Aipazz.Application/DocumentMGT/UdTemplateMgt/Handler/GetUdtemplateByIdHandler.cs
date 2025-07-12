using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.UdTemplateMgt.Queries;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Handler
{
    public class GetUdtemplateByIdHandler : IRequestHandler<GetUdtemplateByIdQuery, Udtemplate?>
    {
        private readonly IUdtemplateRepository _repository;

        public GetUdtemplateByIdHandler(IUdtemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Udtemplate?> Handle(GetUdtemplateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTemplateByIdAndUserId(request.Id, request.UserId);
        }
    }
}