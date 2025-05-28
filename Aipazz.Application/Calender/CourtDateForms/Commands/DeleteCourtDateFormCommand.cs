using MediatR;
using System;

namespace Aipazz.Application.Calendar.CourtDateForms.Commands
{
    public class DeleteCourtDateFormCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteCourtDateFormCommand(Guid id)
        {
            Id = id;
        }
    }
}