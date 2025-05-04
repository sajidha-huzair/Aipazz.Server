using Aipazz.Application.DocumentMgt.Queries;

using Aipazz.Domian.DocumentMgt;
using MediatR;
using Aipazz.Application.DocumentMGT.Interfaces;

namespace Aipazz.Application.DocumentMgt.Handlers
{
    public class GetAllDocumentsHandler : IRequestHandler<GetAllDocumentsQuery, List<Document>>
    {
        private readonly IdocumentRepository _repository;

        public GetAllDocumentsHandler(IdocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Document>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllByUserIdAsync(request.UserId);
        }
    }
}
