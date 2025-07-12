using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands
{
    public record DeleteUdtemplateCommand(string Id, string UserId) : IRequest<Unit>;
}