namespace Aipazz.Domian.client
{
    public class Client
    {
        public string? id { get; set; }
<<<<<<< Updated upstream
        public string? name { get; set; }
=======
        public string UserId { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public string? Mobile { get; set; }
        public string? Landphone { get; set; }
>>>>>>> Stashed changes
        public string? nic { get; set; } // Partition key
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}