using Aipazz.Application.Matters.matter.Queries;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian.Matters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Handlers
{
    public class GetMatterByIdHandler : IRequestHandler<GetMatterByIdQuery, Matter>
    {
        private readonly IMatterRepository _repository;

        public GetMatterByIdHandler(IMatterRepository repository)
        {
            _repository = repository;
        }

        public async Task<Matter> Handle(GetMatterByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMatterById(request.Id, request.ClientNic, request.UserId);
        }
    }
}
