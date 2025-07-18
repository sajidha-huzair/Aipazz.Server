using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.client.Queries;
using Aipazz.Application.client.Interfaces;

namespace Aipazz.Application.client.Handlers
{
    public class CheckClientNICExistsQueryHandler : IRequestHandler<CheckClientNICExistsQuery, bool>
    {
        private readonly IClientRepository _repository;

        public CheckClientNICExistsQueryHandler(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckClientNICExistsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.DoesClientExistByNIC(request.NIC, request.UserId);
        }
    }
}
