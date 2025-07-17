using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Queries
{
    public record GetAllUdtemplatesQuery(string UserId) : IRequest<List<Udtemplate>>;
}