using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Commands
{
    public record UpdateTemplateCommand(Template Template) : IRequest<Unit>;
}
    