using MediatR;
using System.Collections.Generic;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Queries
{
    public class GetClientByNameQuery : IRequest<Client>
    {
<<<<<<< Updated upstream
        public string? Name { get; set; }
=======
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }

        public GetClientByNameQuery(string firstName, string lastName, string userId)
        {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
        }
>>>>>>> Stashed changes
    }

    
}