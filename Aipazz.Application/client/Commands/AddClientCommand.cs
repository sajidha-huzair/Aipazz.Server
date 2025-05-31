using Aipazz.Domian.client;
using MediatR;

namespace Aipazz.Application.client.Commands
{
    public class AddClientCommand : IRequest<Client>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public string? Mobile { get; set; }
        public string? Landphone { get; set; }
        public string? Nic { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? CaseNumber { get; set; }
        public string? CaseName { get; set; }
    }
}