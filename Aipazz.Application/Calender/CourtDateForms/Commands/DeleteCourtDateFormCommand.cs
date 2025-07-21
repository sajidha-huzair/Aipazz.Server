using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Commands
{
    public class DeleteCourtDateFormCommand : IRequest<CourtDateForm?>
    {
        public Guid Id { get; set; }

        public DeleteCourtDateFormCommand(Guid id)
        {
            Id = id;
        }
    }
}