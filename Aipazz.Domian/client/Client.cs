namespace Aipazz.Domian.client
{
    public class Client
    {
        public string? id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public string? Mobile { get; set; }
        public string? Landphone { get; set; }
        public string? nic { get; set; } // Partition key
        public string? email { get; set; }
        public string? Address { get; set; }
        public string? CaseNumber { get; set; }
        public string? CaseName { get; set; }
    }
}