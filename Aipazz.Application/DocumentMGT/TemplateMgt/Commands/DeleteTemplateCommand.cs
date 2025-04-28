using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.DocumentMGT.TemplateMgt.Commands
{
    public record DeleteTemplateCommand(string Id) : IRequest<Unit>;

}
