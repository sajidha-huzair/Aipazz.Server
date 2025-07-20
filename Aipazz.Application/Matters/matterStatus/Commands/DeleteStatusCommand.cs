using System;
using System.Collections.Generic;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Commands
{
    public class DeleteStatusCommand : IRequest<bool>
    {
        public DeleteStatusCommand(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; 
    }
}


