using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Queries
{
    public record GetTemplateByIdQuery(string Id) : IRequest<Template?>;
}
