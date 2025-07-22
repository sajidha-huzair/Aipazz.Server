using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Commands
{
    public class UpdateMatterTypeCommand : IRequest<MatterType>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
