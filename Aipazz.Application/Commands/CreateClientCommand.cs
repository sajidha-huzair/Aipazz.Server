using MediatR;

namespace Aipazz.Application.Commands
{
    public class CreateClientCommand : IRequest<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string CaseNumber { get; set; }
        public string CaseName { get; set; }
        public string NIC { get; set; }
        public string CaseType { get; set; }
    }
}