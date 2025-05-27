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
        public string Id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty; // New field for partition key
    }
}

