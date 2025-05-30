using Aipazz.Application.Calender.Interfaces;
using Aipazz.Application.Calendar.CourtDateForms.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Calendar.CourtDateForms.Handlers
{
    public class DeleteCourtDateFormCommandHandler : IRequestHandler<DeleteCourtDateFormCommand, bool>
    {
        private readonly ICourtDateFormRepository _repository;

        public DeleteCourtDateFormCommandHandler(ICourtDateFormRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(DeleteCourtDateFormCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.DeleteCourtDateForm(request.Id));
        }
    }
}