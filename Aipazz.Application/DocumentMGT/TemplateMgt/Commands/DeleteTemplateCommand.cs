using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Commands
{
    public record DeleteTemplateCommand(string Id) : IRequest<Unit>;
}
