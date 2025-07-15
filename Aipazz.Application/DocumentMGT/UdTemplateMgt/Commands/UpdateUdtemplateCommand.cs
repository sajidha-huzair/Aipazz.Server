using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands
{
    public record UpdateUdtemplateCommand(Udtemplate Udtemplate) : IRequest<Unit>;
}