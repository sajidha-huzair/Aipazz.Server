using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matterStatus.Queries
{
    public class GetStatusByIdQuery : IRequest<Status>
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public GetStatusByIdQuery(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
