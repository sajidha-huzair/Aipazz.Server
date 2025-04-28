namespace Aipazz.Domian.Entities
{
    public class Client
    {
        public string Id { get; set; } // Unique identifier for Cosmos DB
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string CaseNumber { get; set; }
        public string CaseName { get; set; }
        public string NIC { get; set; } // Mandatory
        public string CaseType { get; set; }

        // Parameterless constructor for Cosmos DB
        public Client()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}