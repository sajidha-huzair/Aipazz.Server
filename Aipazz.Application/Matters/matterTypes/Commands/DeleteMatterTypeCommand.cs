using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.Matters.matterTypes.Commands
{
    public class DeleteMatterTypeCommand : IRequest<bool>
    {
        public string Id { get; }
        public string UserId { get; }

        public DeleteMatterTypeCommand(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
