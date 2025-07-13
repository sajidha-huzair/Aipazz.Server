using Aipazz.Application.client.Interfaces;
using Aipazz.Application.client.Queries;
using Aipazz.Domian.client;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Handlers
{
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Client>
    {
        private readonly IClientRepository _repository;

        public GetClientByIdQueryHandler(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, request.Nic, request.UserId);
        }
    }
}