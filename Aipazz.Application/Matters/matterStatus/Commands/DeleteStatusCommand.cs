using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Commands
{
    public class DeleteStatusCommand : IRequest<bool>
    {
        public DeleteStatusCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; } = string.Empty;
    }
}


