using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMgt.Queries
{
    public class GetAllDocumentsQuery : IRequest<List<Document>>
    {
        public string UserId { get; set; }

        public GetAllDocumentsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
