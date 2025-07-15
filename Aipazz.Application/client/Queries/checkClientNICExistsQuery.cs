using MediatR;

namespace Aipazz.Application.client.Queries
{
    public class CheckClientNICExistsQuery : IRequest<bool>
    {
        public string NIC { get; set; }
        public string UserId { get; set; }

        public CheckClientNICExistsQuery(string nic, string userId)
        {
            NIC = nic;
            UserId = userId;
        }
    }
}
