using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.UdTemplateMgt.Commands
{
    public record CreateUdtemplateCommand(Udtemplate Udtemplate) : IRequest<string>;
}
