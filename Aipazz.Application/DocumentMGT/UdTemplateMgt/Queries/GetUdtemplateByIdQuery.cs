using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Queries
{
    public record GetUdtemplateByIdQuery(string Id, string UserId) : IRequest<Udtemplate?>;
}