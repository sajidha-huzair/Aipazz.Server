using Aipazz.Domian.Billing;
using Aipazz.Domian.client;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.client.Queries
{
<<<<<<< Updated upstream
  
        public record GetAllClientsQuery() : IRequest<List<Client>>;
    
}
=======
    public class GetAllClientsQuery : IRequest<List<Client>>
    {
        public string UserId { get; set; }

        public GetAllClientsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
>>>>>>> Stashed changes
