using Aipazz.Application.Calender.CourtDateForms.Commands;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;
using MediatR;

namespace Aipazz.Application.Calender.CourtDateForms.Handlers
{
    public class DeleteCourtDateFormCommandHandler : IRequestHandler<DeleteCourtDateFormCommand, CourtDateForm?>
    {
        private readonly ICourtDateFormRepository _repository;

        public DeleteCourtDateFormCommandHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<CourtDateForm?> Handle(DeleteCourtDateFormCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteCourtDateForm(request.Id);
        }
    }
}